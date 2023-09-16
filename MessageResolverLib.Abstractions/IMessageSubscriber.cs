namespace MessageResolverLib.Abstractions
{
    public interface IMessageSubscriber
    {
        public void Subscribe(string target);
        public string RequestMessage();
    }
}
