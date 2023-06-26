using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHosts
{
    public class ScopedExampleHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ScopedExampleHostedService> _logger;

        public ScopedExampleHostedService(
            ILogger<ScopedExampleHostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("---------------Scoped Services Host-----------------");

            _logger.LogCritical("---------------Scope 1-----------------");
            CallScopeServices();

            _logger.LogCritical("---------------Scope 2-----------------");
            CallScopeServices();

            _logger.LogCritical("---------------Scope 3-----------------");
            CallScopeServices();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void CallScopeServices()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var transiendService = scope!.ServiceProvider.GetRequiredService<IService>();
                transiendService.WriteMessage();

                var transiendService2 = scope!.ServiceProvider.GetRequiredService<IService>();
                transiendService2.WriteMessage();

                var scopedService = scope!.ServiceProvider.GetRequiredService<IScopedService>();
                scopedService.WriteMessage();

                var singletonService = scope!.ServiceProvider.GetRequiredService<ISingletonService>();
                singletonService.WriteMessage();
            }
        }
    }
}
