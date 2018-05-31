using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Core
{
    public class EchoProtocolAdapter : ISlarkProtocolAdapter
    {
        public Task OnConnected(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(true);
        }

        public Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(true);
        }

        public async Task OnReceived(SlarkClientConnection slarkClientConnection, string message)
        {
            await slarkClientConnection.SendAsync(message);
        }
    }
}
