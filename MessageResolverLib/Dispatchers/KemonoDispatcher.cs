using Config;
using MessageResolverLib.Handlers;
using MessageResolverLib.Recipients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageResolverLib.Dispatchers
{
    public class KemonoDispatcher : IMessageDispatcher<ShimahaiRecipient>
    {
        private readonly AppSettings _options;
        private readonly ILogger<KemonoDispatcher> _logger;
        private readonly FriendQueryHandler _query;

        public KemonoDispatcher(
            IOptions<AppSettings> options,
            ILogger<KemonoDispatcher> logger,
            FriendQueryHandler query
        )
        {
            _options = options.Value;
            _logger = logger;
            _query = query;
        }

        private readonly string[] _triggers = { "friends", "friend", "浮莲子", "フレンズ" };

        public bool CanDispatch { get; set; } = true;

        public void DispatchMessage(MessagePackage package)
        {
            if (string.IsNullOrWhiteSpace(package.Message))
                return;
            var span = package.Message.ToLower().AsSpan().Trim();
            if (span.StartsWith(_options.CommandTrigger))
            {
                span = span[_options.CommandTrigger.Length..].TrimStart();
                foreach (var trigger in _triggers)
                {
                    if (span.StartsWith(trigger))
                    {
                        _logger.LogInformation(
                            $"Dispatch information <{nameof(FriendQueryHandler)}>: {span.ToString()}"
                        );
                        _ = _query.HandleMessageAsync(
                            new()
                            {
                                Message = span[trigger.Length..].TrimStart().ToString(),
                                Messenger = package.Messenger
                            }
                        );
                        break;
                    }
                }
            }
        }
    }
}
