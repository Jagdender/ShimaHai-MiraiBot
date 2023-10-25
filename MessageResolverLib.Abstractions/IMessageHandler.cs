namespace MessageResolverLib
{
    public interface IMessageHandler
    {
        public bool CanHandle { get; set; }

        public bool IsRedirective { get; set; }

        public long RedirectTarget { get; set; }

        public Task HandleMessageAsync(MessagePackage message);
    }
}
