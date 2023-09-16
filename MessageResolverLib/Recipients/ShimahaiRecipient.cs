using Config;
using MeowMiraiLib;
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MessageResolverLib.Recipients
{
    public class ShimahaiRecipient : IMessageRecipient
    {
        private readonly IServiceProvider _services;

        public ShimahaiRecipient(IOptions<AppSettings> options, IServiceProvider services)
        {
            _services = services;
        }

        public void ReceiveMessage(Sender sender, Message[] messages)
        {
            if (sender is not GroupMessageSender messenger || messages is null || !messages.Any())
                return;

            string text = messages.MGetPlainString();
            if (string.IsNullOrWhiteSpace(text))
                return;

            var dispatchers = GetDispatchers();
            foreach (var dispatcher in dispatchers)
            {
                dispatcher.DispatchMessage(
                    new MessagePackage() { Message = text, Messenger = messenger.group.id }
                );
            }
        }

        public IEnumerable<IMessageDispatcher> GetDispatchers()
        {
            var services = _services.GetServices<IMessageDispatcher<ShimahaiRecipient>>();
            return services;
        }
    }
}
