using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;

namespace MessageResolverLib
{
    public interface IMessageRecipient
    {
        public void ReceiveMessage(Sender sender, Message[] messages);
        public IEnumerable<IMessageDispatcher> GetDispatchers();
    }
}
