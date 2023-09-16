using Config;
using MeowMiraiLib;
using Microsoft.Extensions.Options;

namespace MiraiClient
{
    public class ShimahaiClient
    {
        public ShimahaiClient(IOptions<AppSettings> options)
        {
            Client = new(
                options.Value.Login.Host,
                options.Value.Login.Port,
                options.Value.Login.VerifyKey,
                options.Value.Login.QQ
            );
        }

        public Client Client { get; init; }

        public Task<bool> StartAsync()
        {
            return Client.ConnectAsync();
        }
    }
}
