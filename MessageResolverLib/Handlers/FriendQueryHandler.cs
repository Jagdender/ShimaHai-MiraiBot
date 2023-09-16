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
        private readonly IFriendController _controller;
        private readonly AppSettings _options;
        private readonly ShimahaiClient _client;
        private readonly IMessageSender _messenger;
        private readonly ILogger<FriendQueryHandler> _logger;

        public FriendQueryHandler(
            IFriendController controller,
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

        public async Task HandleMessageAsync(MessagePackage package)
        {
            if (!CanHandle)
                return;
            Friend? friend = null;
            bool isHC = package.Message.EndsWith(" hc");
            var parameter = isHC
                ? package.Message[..(package.Message.Length - 2)].TrimEnd()
                : package.Message;
            _logger.LogInformation("Parameter: {arg} , IsHC:{isHC}", parameter, isHC);

            if (int.TryParse(parameter, out int id))
                friend = await _controller.GetFriend(id);
            else
                friend = await _controller.GetFriend(parameter, isHC: isHC);
            if (friend is null)
                _ = _messenger.AddText("没有找到你说的Friendね").SendMessageAsync(package.Messenger);
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
                    _ = m.AddImage(imagePath: path).SendMessageAsync(package.Messenger);
                else
                    _ = m.AddText("\n这个Friend还没有注册图片ね~").SendMessageAsync(package.Messenger);
            }
        }
    }
}
