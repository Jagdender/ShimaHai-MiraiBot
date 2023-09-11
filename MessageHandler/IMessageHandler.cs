using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public interface IMessageHandler
    {
        public Task HandleMessageAsync(string message);
    }
}
