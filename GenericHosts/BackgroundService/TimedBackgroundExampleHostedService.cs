using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHosts.BackgroundService
{
    public class TimedBackgroundExampleHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;

        private Timer _timer;

        public TimedBackgroundExampleHostedService(
            ILogger<TimedBackgroundExampleHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Background service started]");

            _timer = new Timer(LogSomething, "[Timed action executed]", TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Background service stopped]");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void LogSomething(object message)
        {
            _logger.LogInformation(message.ToString());
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
