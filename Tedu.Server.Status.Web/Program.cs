using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Tedu.Server.Status.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IHost host = CreateHostBuilder(args).Build();
                Console.WriteLine("Starting up");
                host.Run();
                Console.WriteLine("Shutting down");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Terminating due to an unhandled exception: " + ex);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webHostBuilder => webHostBuilder
                        .UseStartup<Startup>());
        }
    }
}
