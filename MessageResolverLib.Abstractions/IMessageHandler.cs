namespace MessageResolverLib
{
    public interface IMessageHandler
    {
        public bool CanHandle { get; set; }
        public Task HandleMessageAsync(MessagePackage message);
    }
}
