using MeowMiraiLib;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimaHai
{
    internal class HostService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Global.G_Client = new("");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Application has shut down, press any key to exit.");

            Console.ReadKey();
        }
    }
}
