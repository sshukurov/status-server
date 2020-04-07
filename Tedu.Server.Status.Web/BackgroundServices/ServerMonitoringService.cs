using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tedu.Server.Status.DataAccess.Commands;
using Tedu.Server.Status.DataAccess.Entities;
using Tedu.Server.Status.DataAccess.Queries;

namespace Tedu.Server.Status.Web.BackgroundServices
{
    public class ServerMonitoringService : TimedHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<string, ServerHttpProbeService> _probeServiceByHost = new Dictionary<string, ServerHttpProbeService>();

        private Task<List<Probe>[]> _whenAllProbesFinishTask;

        public ServerMonitoringService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override int IntervalSeconds { get; } = 60;

        protected override async Task DoWorkAsync()
        {
            if (_whenAllProbesFinishTask?.IsCompleted == false)
            {
                Console.WriteLine("Skipping probe cycle since previous cycle is in progress.");
                return;
            }

            DataAccess.Entities.Server[] servers;
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var serverQueries = scope.ServiceProvider.GetService<IServerQueries>();
                servers = await serverQueries.GetAllServersAsync();
            }
            Console.WriteLine("Servers discovered: {0}.", servers.Length);
            Array.ForEach(servers, x => _probeServiceByHost[x.Host] = new ServerHttpProbeService(x));

            DateTime checkedDateTimeUtc = DateTime.UtcNow;
            IEnumerable<Task<List<Probe>>> tasksQuery = servers.AsParallel().Select(
                async x =>
                {
                    string host = x.Host;
                    var probes = new List<Probe>();

                    ServerHttpProbeService probeService = _probeServiceByHost[host];
                    Probe isHostReachableProbe = await probeService.CheckHostIsReachableAsync(checkedDateTimeUtc);
                    probes.Add(isHostReachableProbe);
                    if (isHostReachableProbe.Result == ProbeResult.Failure)
                    {
                        return probes;
                    }

                    probes.Add(
                        await probeService.PerformProbeAsync(
                            ProbeType.IsSslCertificateValid,
                            string.Empty,
                            checkedDateTimeUtc));

                    probes.Add(
                        await probeService.PerformProbeAsync(
                            ProbeType.IsTutoringApiAvailable,
                            "api/account/verifyemail?email=status@tedu.app",
                            checkedDateTimeUtc));

                    probes.Add(
                        await probeService.PerformProbeAsync(
                            ProbeType.IsAdminApiAvailable,
                            "api-admin/api/account/verifyemail?email=status@tedu.app",
                            checkedDateTimeUtc));

                    probes.Add(
                        await probeService.PerformProbeAsync(
                            ProbeType.IsTutoringSwaggerAvailable,
                            "swagger/index.html",
                            checkedDateTimeUtc));

                    probes.Add(
                        await probeService.PerformProbeAsync(
                            ProbeType.IsAdminSwaggerAvailable,
                            "api-admin/swagger/index.html",
                            checkedDateTimeUtc));
                    return probes;
                });
            _whenAllProbesFinishTask = Task.WhenAll(tasksQuery);
            List<Probe>[] probeSets = await _whenAllProbesFinishTask;
            Probe[] flattenedProbes = probeSets.SelectMany(x => x).ToArray();

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var probeCommands = scope.ServiceProvider.GetService<IProbeCommands>();
                await probeCommands.AddRangeAsync(flattenedProbes);
            }
        }

        public override void Dispose()
        {
            foreach (ServerHttpProbeService probeService in _probeServiceByHost.Values)
            {
                probeService.Dispose();
            }
        }
    }
}
