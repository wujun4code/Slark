using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class EchoProtocol : ISlarkProtocol
    {
        public Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            return context.Sender.ToEnumerableAsync();
        }

        public Task<string> SerializeAsync(ISlarkMessage message)
        {
            return Task.FromResult(message.MetaText);
        }
    }
}
