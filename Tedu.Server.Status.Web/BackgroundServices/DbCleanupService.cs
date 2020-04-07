using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tedu.Server.Status.DataAccess.Commands;

namespace Tedu.Server.Status.Web.BackgroundServices
{
    public class DbCleanupService : TimedHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DbCleanupService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override int IntervalSeconds { get; } = 1 * 60 * 60; // 1 hour interval

        protected override async Task DoWorkAsync()
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var probeCommands = scope.ServiceProvider.GetService<IProbeCommands>();
                var backupCommands = scope.ServiceProvider.GetService<IBackupCommands>();
                DateTime twoWeeksBefore = DateTime.UtcNow.AddDays(-14);
                await probeCommands.RemoveAsync(x => x.CheckedDateTimeUtc < twoWeeksBefore);
                await backupCommands.RemoveAsync(x => x.CreatedDateTimeUtc < twoWeeksBefore);
            }
        }
    }
}
