using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GenericHosts
{
    public class Service2 : IService
    {
        private readonly ExampleHostedService.ExampleHostedServiceOptions _options;
        private readonly ILogger _logger;
        private readonly IScopedService _scopedService;
        private readonly ISingletonService _singletonService;

        public Guid Id { get; set; } = Guid.NewGuid();

        public Service2 (
            IOptions<ExampleHostedService.ExampleHostedServiceOptions> options,
            ILogger<Service2> logger,
            IScopedService scopedService,
            ISingletonService singletonService)
        {
            _options = options.Value;
            _logger = logger;
            _scopedService = scopedService;
            _singletonService = singletonService;
        }

        public void WriteMessage()
        {
            _logger.LogWarning($"[Service log] Service2 options = {_options?.Name} {_options?.Age}");
            _logger.LogWarning($"[Transiend service 2] Id: {Id}");

            _scopedService.WriteMessage();

            _singletonService.WriteMessage();
        }
    }
}
