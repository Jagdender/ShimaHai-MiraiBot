namespace MessageResolverLib;

public readonly struct MessagePackage
{
    public string Message { get; init; }
    public long Messenger { get; init; }
}
