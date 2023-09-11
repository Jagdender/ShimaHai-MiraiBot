using Mirai.CSharp.HttpApi.Handlers;
using Mirai.CSharp.HttpApi.Models.EventArgs;
using Mirai.CSharp.HttpApi.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ShimaHai.Handlers
{
    internal class DisconnectHandler : IMiraiHttpMessageHandler<IDisconnectedEventArgs>
    {
        public async Task HandleMessageAsync(IMiraiHttpSession session, IDisconnectedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await session.ConnectAsync(e.LastConnectedQQNumber);
                    e.BlockRemainingHandlers = true;
                    break;
                }
                catch (ObjectDisposedException) // session 已被释放
                {
                    break;
                }
                catch (Exception)
                {
                    i++;
                    await Task.Delay(1000);
                }
            }
        }
    }
}
