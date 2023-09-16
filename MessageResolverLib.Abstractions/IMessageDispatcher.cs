namespace MessageResolverLib
{
    public interface IMessageDispatcher
    {
        public bool CanDispatch { get; set; }
        public void DispatchMessage(MessagePackage package);
    }

    public interface IMessageDispatcher<IRecipient> : IMessageDispatcher
        where IRecipient : class, IMessageRecipient { }
}
