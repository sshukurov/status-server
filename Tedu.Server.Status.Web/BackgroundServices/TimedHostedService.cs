using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tedu.Server.Status.Web.BackgroundServices
{
    /// <summary>
    /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.0&tabs=visual-studio#timed-background-tasks"/>
    /// </summary>
    public abstract class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;

        protected abstract int IntervalSeconds { get; }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(
                callback: async state => await DoWorkAsync(),
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromSeconds(IntervalSeconds));

            return Task.CompletedTask;
        }

        protected abstract Task DoWorkAsync();

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, period: 0);

            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
