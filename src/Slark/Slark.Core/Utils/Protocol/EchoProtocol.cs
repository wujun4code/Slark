using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class EchoProtocol : ISlarkProtocol
    {
        public Task ExecuteAsync(SlarkContext context)
        {
            context.HasNotice = false;
            return Task.FromResult(context.Message.MetaText);
        }
    }
}
