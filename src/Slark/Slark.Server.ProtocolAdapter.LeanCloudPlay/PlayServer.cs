using Slark.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayServer : SlarkServer
    {
        public PlayServer()
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
            return Task.FromResult(true);
        }

        public override Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(false);
        }

        public override async Task OnReceived(SlarkClientConnection slarkClientConnection, string message)
        {
            await this.Broadcast(message);
            //await slarkClientConnection.SendAsync(message);
        }
    }
}
