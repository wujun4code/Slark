using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class DirectProtocol : ISlarkProtocol
    {
        public Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            return Task.FromResult(context.Server.Connections as IEnumerable<SlarkClientConnection>);
        }

        public Task<string> NotifyAsync(SlarkContext context)
        {
            return Task.FromResult(context.Message.MetaText);
        }

        public Task<string> ResponseAsync(SlarkContext context)
        {
            context.HasNotice = true;
            return Task.FromResult(context.Message.MetaText);
        }
    }
}
