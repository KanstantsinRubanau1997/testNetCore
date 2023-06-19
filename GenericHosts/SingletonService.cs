using Microsoft.Extensions.Logging;

namespace GenericHosts
{
    public class SingletonService : ISingletonService
    {
        private readonly ILogger _logger;

        public Guid Id { get; set; } = Guid.NewGuid();

        public SingletonService(ILogger<SingletonService> logger)
        {
            _logger = logger;
        }

        public void WriteMessage()
        {
            _logger.LogCritical($"[Singleton service] Id: {Id}");
        }
    }
}
