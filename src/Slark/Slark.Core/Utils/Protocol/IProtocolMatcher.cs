using System;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public interface IProtocolMatcher
    {
        Task<ISlarkProtocol> FindAsync(SlarkContext context);
    }
}
