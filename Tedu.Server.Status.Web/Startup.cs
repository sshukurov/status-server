using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tedu.Server.Status.DataAccess;
using Tedu.Server.Status.DataAccess.Commands;
using Tedu.Server.Status.DataAccess.Queries;
using Tedu.Server.Status.Web.BackgroundServices;
using Tedu.Server.Status.Web.Configuration;

namespace Tedu.Server.Status.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Settings _settings;

        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            Configuration = configuration;
            _settings = new Settings();
            Configuration.Bind(_settings);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(c =>
                {
                    c.EnableEndpointRouting = false;
                })
                .AddJsonOptions(options =>
                {
                    JsonSerializerOptions serializerOptions = options.JsonSerializerOptions;
                    serializerOptions.Converters.Add(new JsonStringEnumConverter());
                    serializerOptions.IgnoreNullValues = true;
                });

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<TeduStatusDbContext>((provider, options) =>
                    options.UseNpgsql(provider.GetService<IDatabaseSettings>().GetConnectionString(),
                    o => o.MigrationsAssembly("Tedu.Server.Status.DataAccess.Migrations")))

                .AddSingleton<IDatabaseSettings>(_settings.DatabaseSettings)
                .AddSingleton<IHostSettings>(_settings.HostSettings)

                .AddScoped<IServerQueries, ServerQueries>()
                .AddScoped<IServerCommands, ServerCommands>()
                .AddScoped<IProbeQueries, ProbeQueries>()
                .AddScoped<IProbeCommands, ProbeCommands>()
                .AddScoped<IBackupQueries, BackupQueries>()
                .AddScoped<IBackupCommands, BackupCommands>()

                .AddHostedService<ServerMonitoringService>()
                .AddHostedService<DbCleanupService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("doc", new OpenApiInfo { Title = "TEDU Status API", Version = "0.1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c => c.PreSerializeFilters.Add((openApiDocument, httpRequest) =>
                    openApiDocument.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer
                        {
                            Url = $"https://{_settings.Host}"
                        }
                    }))
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/doc/swagger.json", "TEDU Status API 0.1"));
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
