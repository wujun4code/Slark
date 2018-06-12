using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Slark.Core.Protocol
{
    public interface ISlarkProtocol
    {
        Task<string> ResponseAsync(SlarkContext context);
        Task<string> NotifyAsync(SlarkContext context);
        Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context);
    }
}
