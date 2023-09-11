using Mirai.CSharp.HttpApi.Models.EventArgs;
using Mirai.CSharp.HttpApi.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShimaHai.Parsers
{
    internal class UnnamedParser : IMiraiHttpMessageParser<IFriendEventArgs>
    {
        public Type MessageType => throw new NotImplementedException();

        public bool CanParse(in JsonElement root)
        {
            throw new NotImplementedException();
        }

        public IFriendEventArgs Parse(in JsonElement root)
        {
            throw new NotImplementedException();
        }
    }
}
