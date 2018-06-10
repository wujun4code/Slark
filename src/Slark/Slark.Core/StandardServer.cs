using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core.Protocol;
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
        public virtual IProtocolMatcher ProtocolMatcher { get; set; }
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
            var context = new SlarkContext()
            {
                Server = this,
                Message = new SlarkStandardMessage()
                {
                    MetaText = message
                },
                Sender = slarkClientConnection,
            };
            var processor = await ProtocolMatcher.FindAsync(context);
            context.Notice = await processor.SerializeAsync(context.Message);
            context.Receivers = await processor.GetTargetsAsync(context);
            await context.PushNoticeAsync();
        }

        public override Task<string> OnRPC(string method, params object[] rpcParamters)
        {
            return this.RPCAllAsync(method, rpcParamters);
        }

    }
}
