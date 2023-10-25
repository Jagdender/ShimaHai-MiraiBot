using Config;
using MeowMiraiLib;
using MeowMiraiLib.Msg.Type;
using MessageResolverLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiraiClient;
using TwitterFetcher;

namespace ShimaHai
{
    internal class HostService : IHostedService
    {
        private readonly ShimahaiClient _client;
        private readonly IMessageRecipient _recipient;
        private readonly IHostApplicationLifetime _application;
        private readonly ILogger<HostService> _logger;
        private readonly TwitterFetchEngine _fetchEngine;
        private readonly AppSettings _options;

        public HostService(
            ShimahaiClient client,
            IMessageRecipient recipient,
            IHostApplicationLifetime application,
            ILogger<HostService> logger,
            TwitterFetchEngine fetchEngine,
            IOptions<AppSettings> options
        )
        {
            _client = client;
            _recipient = recipient;
            _application = application;
            _logger = logger;
            _fetchEngine = fetchEngine;
            _options = options.Value;
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

            await _fetchEngine.InitializeAsync();
            _fetchEngine.Subscribe(_options.Twitter.Subscribers);

            _fetchEngine.OutputFetchedTweets += (tweets) =>
                TempSender(tweets, _options.Targets.Kemono);

            _ = _fetchEngine.FetchAsync((tweets) => { });

            _fetchEngine.Start(TimeSpan.FromMinutes(30));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _fetchEngine.DisposeAsync();

            Console.WriteLine("Application has shut down, press any key to exit.");

            Console.ReadKey();
        }

        private void TempSender(IEnumerable<string> tweets, long target)
        {
            foreach (var tweet in tweets)
            {
                new Message[] { new Image(path: tweet) }.SendToGroup(target, _client.Client);
            }
        }
    }
}
