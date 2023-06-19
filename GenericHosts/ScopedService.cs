using Microsoft.Extensions.Logging;

namespace GenericHosts
{
    public class ScopedService : IScopedService
    {
        private readonly ILogger _logger;

        public Guid Id { get; set; } = Guid.NewGuid();

        public ScopedService(ILogger<ScopedService> logger)
        {
            _logger = logger;
        }

        public void WriteMessage()
        {
            _logger.LogCritical($"[Scoped service] Id: {Id}");
        }
    }
}
