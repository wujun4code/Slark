using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class DirectProtocol : ISlarkProtocol
    {
        public Task ExecuteAsync(SlarkContext context)
        {
            context.HasNotice = true;
            context.Notice = context.Message.MetaText;
            return Task.FromResult(context.Notice);
        }
    }
}
