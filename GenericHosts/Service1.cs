using Microsoft.Extensions.Logging;

namespace GenericHosts
{
    public class Service1 : IService
    {
        private readonly ILogger _logger;
        private readonly IScopedService _scopedService;
        private readonly ISingletonService _singletonService;

        public Guid Id { get; set; } = Guid.NewGuid();

        public Service1(ILogger<Service1> logger, IScopedService scopedService, ISingletonService singletonService)
        {
            _logger = logger;
            _scopedService = scopedService;
            _singletonService = singletonService;
        }

        public void WriteMessage()
        {
            _logger.LogWarning($"[Transiend service 1] Id: {Id}");

            _scopedService.WriteMessage();

            _singletonService.WriteMessage();
        }
    }
}
