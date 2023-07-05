using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GenericHosts
{
    public class ScopedService : IScopedService
    {
        private readonly ILogger _logger;
        private readonly ScopedServiceOptions _options;

        public Guid Id { get; set; } = Guid.NewGuid();

        public ScopedService(
            ILogger<ScopedService> logger,
            IOptionsSnapshot<ScopedServiceOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public void WriteMessage()
        {
            _logger.LogCritical($"[Scoped service] Id: {Id}");

            for (var i = 0; i < 3; i++)
            {
                _logger.LogCritical($"[Scoped service] [Options] Id: {_options.Id}");
            }
        }

        public class ScopedServiceOptions
        {
            public Guid Id { get; set; } = Guid.NewGuid();
        }
    }
}
