using Config;
using MessageResolverLib.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiraiClient;
using ShimahaiDatabase.Controllers;
using ShimahaiDatabase.Models;

namespace MessageResolverLib.Handlers
{
    public class FriendQueryHandler : IMessageHandler
    {
        private readonly FriendController _controller;
        private readonly AppSettings _options;
        private readonly ShimahaiClient _client;
        private readonly IMessageSender _messenger;
        private readonly ILogger<FriendQueryHandler> _logger;

        public FriendQueryHandler(
            FriendController controller,
            IOptionsSnapshot<AppSettings> options,
            ShimahaiClient client,
            IMessageSender messenger,
            ILogger<FriendQueryHandler> logger
        )
        {
            _controller = controller;
            _options = options.Value;
            _client = client;
            _messenger = messenger;
            _logger = logger;
        }

        public bool CanHandle { get; set; } = true;
        public bool IsRedirective { get; set; } = false;
        public long RedirectTarget { get; set; } = 0;

        public async Task HandleMessageAsync(MessagePackage package)
        {
            if (!CanHandle)
                return;
            Friend? friend = null;
            bool isHC = package.Message.EndsWith(" hc");
            var parameter = isHC
                ? package.Message[..(package.Message.Length - 2)].TrimEnd()
                : package.Message;
            _logger.LogInformation(
                "Handler: {handler} , Parameter: {arg} , IsHC: {isHC}",
                nameof(FriendQueryHandler),
                parameter,
                isHC
            );

            if (int.TryParse(parameter, out int id))
                friend = await _controller.GetFriend(id);
            else
                friend = await _controller.GetFriend(parameter, isHC: isHC);
            if (friend is null)
                _ = RespondMessageAsync(_messenger.AddText("没有找到你说的Friendね"), package);
            else
            {
                var m = _messenger
                    .AddText($"{friend.Id}. {friend.Name}\n")
                    .AddText($"[ {friend.NameEn} ]  {friend.NameCn}\n")
                    .AddText("=\n")
                    .AddText($"{friend.Greeting}");

                string path = Path.Combine(
                    _options.Uri.Base,
                    _options.Uri.InfoImage,
                    friend.Id + ".png"
                );
                if (File.Exists(path))
                    _ = RespondMessageAsync(m.AddImage(imagePath: path), package);
                else
                    _ = RespondMessageAsync(m.AddText("\n这个Friend还没有注册图片ね~"), package);
            }
        }

        private Task RespondMessageAsync(IMessageSender message, MessagePackage package)
        {
            if (IsRedirective)
            {
                if (RedirectTarget == 0)
                    throw new ArgumentException("The RedirectTarget hasn't been initialized.");
                else
                    return message.SendMessageAsync(RedirectTarget);
            }
            else
            {
                return message.SendMessageAsync(package.Messenger);
            }
        }
    }
}
