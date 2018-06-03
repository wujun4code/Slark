using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkStandardServer : SlarkServer
    {
        public SlarkStandardServer()
        {
            Connections = new List<SlarkClientConnection>();
        }

        public override List<SlarkClientConnection> Connections
        {
            get;
            set;
        }

        public override Task OnConnected(SlarkClientConnection slarkClientConnection)
        {
            slarkClientConnection.Client = new SlarkStandardClient();
            return Task.FromResult(true);
        }

        public override Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(false);
        }

        public override async Task OnReceived(SlarkClientConnection slarkClientConnection, string message)
        {
            await this.Broadcast(message);
        }

        public abstract override Task<string> OnRPC(string method, string message);
    }
}
