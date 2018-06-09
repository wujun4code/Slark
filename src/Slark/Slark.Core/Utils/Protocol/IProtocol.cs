using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Slark.Core.Utils
{
    public interface ISlarkProtocol
    {
        Task<string> SerializeAsync(ISlarkMessage message);
        Task<ICollection<SlarkClient>> GetTargetsAsync(SlarkServer server);
    }
}
