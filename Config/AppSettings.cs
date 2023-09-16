namespace Config
{
    public sealed class AppSettings
    {
        public required string DbConnectString { get; init; }
        public required string CommandTrigger { get; init; }
        public required Login Login { get; init; }
        public required ImageUri Uri { get; init; }
        public required Targets Targets { get; init; }
        public required Twitter Twitter { get; init; }
    }

    public sealed class ImageUri
    {
        public required string Base { get; init; }
        public required string InfoImage { get; init; }
        public required string TwiImage { get; init; }
        public required string EmojiImage { get; init; }
    }

    public sealed class Login
    {
        public required string Host { get; init; }
        public required int Port { get; init; }
        public required long QQ { get; init; }
        public required string VerifyKey { get; init; }
    }

    public sealed class Targets
    {
        public required long Test { get; init; }
        public required long Kemono { get; init; }
    }

    public sealed class Twitter
    {
        public required string Url { get; init; }
        public required string AuthToken { get; init; }
        public required string Twid { get; init; }
    }
}
