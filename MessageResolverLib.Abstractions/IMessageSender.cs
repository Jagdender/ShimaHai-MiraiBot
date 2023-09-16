namespace MessageResolverLib.Abstractions
{
    public interface IMessageSender
    {
        public Task SendMessageAsync(long target);
        public IMessageSender AddText(params string[] text);
        public IMessageSender AddImage(string? imagePath = null, string? imageUrl = null);
    }
}
