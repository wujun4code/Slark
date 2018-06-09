using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Utils.Protocol
{
    public class DirectProtocol : ISlarkNotice
    {
        public Task<ICollection<SlarkClient>> GetTargetsAsync(SlarkServer server)
        {
            return Task.FromResult(server.Connections as ICollection<SlarkClient>);
        }

        public Task<string> SerializeAsync(ISlarkMessage message)
        {
            return Task.FromResult(message.MetaText);
        }
    }
}
