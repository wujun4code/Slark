using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core.Utils;

namespace Slark.Core
{
    public class SlarkStandardServer : SlarkServer
    {
        public SlarkStandardServer()
        {
            Connections = new List<SlarkClientConnection>();
            Pollings = new List<SlarkPollingController>();
        }

        public List<SlarkPollingController> Pollings { get; set; }

        public void AddPolling(SlarkPollingController slarkPollingController)
        {
            slarkPollingController.Server = this;
            Pollings.Add(slarkPollingController);
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
            await this.BroadcastAsync(message);
        }

        public override Task<string> OnRPC(string method, params object[] rpcParamters)
        {
            return this.RPCAllAsync(method, rpcParamters);
        }

    }
}
