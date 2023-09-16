using MessageResolverLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiraiClient;

namespace ShimaHai
{
    internal class HostService : IHostedService
    {
        private readonly ShimahaiClient _client;
        private readonly IMessageRecipient _recipient;
        private readonly IHostApplicationLifetime _application;
        private readonly ILogger<HostService> _logger;

        public HostService(
            ShimahaiClient client,
            IMessageRecipient recipient,
            IHostApplicationLifetime application,
            ILogger<HostService> logger
        )
        {
            _client = client;
            _recipient = recipient;
            _application = application;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var isSuccess = await _client.StartAsync();
            if (!isSuccess)
            {
                _logger.LogCritical("Bot start failed.");
                _application.StopApplication();
            }

            _client.Client.OnGroupMessageReceive += _recipient.ReceiveMessage;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Application has shut down, press any key to exit.");

            Console.ReadKey();

            return Task.CompletedTask;
        }
    }
}
