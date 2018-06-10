using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Slark.Core.Protocol
{
    public interface ISlarkProtocol
    {
        Task<string> SerializeAsync(ISlarkMessage message);
        Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context);
    }
}
