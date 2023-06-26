using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GenericHosts
{
    public class ExampleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IService _service;
        private readonly IConfiguration _configuration;
        private readonly IScopedService _scopedService;
        private readonly ISingletonService _singletonService;

        public ExampleHostedService(
            ILogger<ExampleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IService service,
            IConfiguration configuration,
            IScopedService scopedService,
            ISingletonService singletonService)
        {
            _logger = logger;
            _service = service;
            _configuration = configuration;
            _scopedService = scopedService;
            _singletonService = singletonService;

            //appLifetime.ApplicationStarted.Register(OnStarted);
            //appLifetime.ApplicationStopping.Register(OnStopping);
            //appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical($"---------------Host {_configuration.GetValue<string>("Counter")}-----------------");
            _logger.LogInformation("1. StartAsync has been called.");

            _logger.LogWarning("[SomeParameter:Value] " + _configuration.GetValue<string>("SomeParameter:Value"));

            _logger.LogWarning("[Overridable Value] " + _configuration.GetValue<string>("OverridableValue"));

            _logger.LogWarning("[Env/Appsettings value] " + _configuration.GetValue<string>("Value"));

            var options = _configuration
                .GetSection(nameof(ExampleHostedServiceOptions))
                .Get<ExampleHostedServiceOptions>();
            if (options != null)
            {
                _logger.LogWarning($"[From Section] Name: {options.Name} Age: {options.Age}");
            }
            else
            {
                _logger.LogCritical("[From Section] Options not found");
            }

            _logger.LogWarning("[In-memory value] " + _configuration.GetValue<string>("InMemoryKey"));

            _service.WriteMessage();

            _scopedService.WriteMessage();

            _singletonService.WriteMessage();

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

        public class ExampleHostedServiceOptions
        {
            public string Name { get; set; }

            [Range(0, 30, ErrorMessage = "Invalid Age")]
            public int Age { get; set; }
        }
    }
}
