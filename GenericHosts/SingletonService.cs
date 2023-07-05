using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GenericHosts
{
    public class SingletonService : ISingletonService
    {
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<SingletonServiceOptions> _options;

        public Guid Id { get; set; } = Guid.NewGuid();

        public SingletonService(
            ILogger<SingletonService> logger,
            IOptionsMonitor<SingletonServiceOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public void WriteMessage()
        {
            _logger.LogCritical($"[Singleton service] Id: {Id}");
            for (var i = 0; i < 3; i++)
            {
                //Thread.Sleep(3000);

                _logger.LogCritical($"[Singleton service] [Options] Id: {_options.CurrentValue.Id}");
            }
        }

        public class SingletonServiceOptions
        {
            public Guid Id { get; set; }// = Guid.NewGuid();
        }
    }
}
