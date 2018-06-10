using System;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class SlarkStandardProtocolMatcher : IProtocolMatcher
    {
        public virtual Task<ISlarkProtocol> FindAsync(SlarkContext context)
        {
            return Task.FromResult(new EchoProtocol() as ISlarkProtocol);
        }
    }
}
