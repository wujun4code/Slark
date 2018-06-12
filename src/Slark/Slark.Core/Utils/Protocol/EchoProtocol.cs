using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class EchoProtocol : ISlarkProtocol
    {
        public Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            throw new NotImplementedException();
        }

        public Task<string> NotifyAsync(SlarkContext context)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResponseAsync(SlarkContext context)
        {
            context.HasNotice = false;
            return Task.FromResult(context.Message.MetaText);
        }
    }
}
