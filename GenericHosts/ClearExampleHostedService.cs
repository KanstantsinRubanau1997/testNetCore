using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHosts
{
    public class ClearExampleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly ILogger _loggerWithCustomCategory;
        private readonly IConfiguration _configuration;

        public ClearExampleHostedService(
            ILogger<ClearExampleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _loggerWithCustomCategory = loggerFactory.CreateLogger("CustomCategory");
            _configuration = configuration;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical($"---------------Host ClearExampleHostedService started-----------------");

            _logger.LogTrace("Service started");
            _loggerWithCustomCategory.LogTrace("Service started");

            _logger.LogInformation("Service started");
            _loggerWithCustomCategory.LogInformation("Service started");

            _logger.Log(LogLevel.Warning, "Service started");
            _loggerWithCustomCategory.Log(LogLevel.Warning, "Service started");

            _logger.LogWarning("[SomeValue] " + _configuration.GetValue<string>("SomeValue"));

            using (_logger.BeginScope("[Additional scope message]"))
            {
                _logger.Log(LogLevel.Warning, new EventId(1000, "Start"), "[From scope]");
                _logger.Log(LogLevel.Warning, new EventId(3000, "End"), "[Another scope message]");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            _logger.LogInformation("3. OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogInformation("5. OnStopped has been called.");
        }
    }
}
