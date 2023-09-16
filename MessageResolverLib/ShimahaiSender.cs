using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Type;
using MessageResolverLib.Abstractions;
using MiraiClient;

namespace MessageResolverLib
{
    public class ShimahaiSender : IMessageSender
    {
        private readonly ShimahaiClient _client;
        private readonly ICollection<Message> _messages = new List<Message>();

        public ShimahaiSender(ShimahaiClient client)
        {
            _client = client;
        }

        public IMessageSender AddImage(string? imagePath = null, string? imageUrl = null)
        {
            if (imagePath is not null)
                _messages.Add(new Image(path: imagePath));
            else if (imageUrl is not null)
                _messages.Add(new Image(url: imageUrl));
            return this;
        }

        public IMessageSender AddText(params string[] text)
        {
            foreach (var message in text)
                _messages.Add(new Plain(message));
            return this;
        }

        public Task SendMessageAsync(long target)
        {
            var messages = _messages.ToArray();
            _messages.Clear();
            return new GroupMessage(target, messages).SendAsync(_client.Client);
        }
    }
}
