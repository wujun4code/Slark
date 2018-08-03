﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core.Protocol;
using Slark.Core.Utils;
using Slark.Core.Utils.Protocol;

namespace Slark.Core
{
    public abstract class SlarkStandardServer : SlarkServer
    {
        public SlarkStandardServer()
        {
            ConnectionList = new ConcurrentHashSet<SlarkClientConnection>();
            Pollings = new List<SlarkPollingController>();
        }

        public virtual List<SlarkPollingController> Pollings { get; set; }
        public virtual IProtocolMatcher ProtocolMatcher { get; set; }
        public void AddPolling(SlarkPollingController slarkPollingController)
        {
            slarkPollingController.Server = this;
            Pollings.Add(slarkPollingController);
        }

        public virtual ConcurrentHashSet<SlarkClientConnection> ConnectionList
        {
            get;
            set;
        }

        public override IEnumerable<SlarkClientConnection> Connections { get => ConnectionList; }
        public override string ServerUrl { get; set; }
        public override string ClientConnectionUrl { get; set; }

        public override async Task<SlarkClientConnection> OnConnected(SlarkClientConnection slarkClientConnection)
        {
            slarkClientConnection.Client = await CreateClient(slarkClientConnection);
            return slarkClientConnection;
        }

        public override Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(false);
        }

        public override async Task OnReceived(SlarkClientConnection sender, string message)
        {
            var context = CreateContext(sender, message);

            var processor = await MatchProtocolAsync(context);

            await processor.ExecuteAsync(context);

            await context.ReplyAsync();

            if (context.HasNotice)
            {
                await context.PushNoticeAsync();
            }
        }

        public virtual SlarkContext CreateContext(SlarkClientConnection slarkClientConnection, string message)
        {
            var context = new StandardContext()
            {
                Server = this,
                Message = CreateMessage(message),
                Sender = slarkClientConnection,
            };
            return context;
        }

        public virtual Task<SlarkClient> CreateClient(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(new SlarkStandardClient() { } as SlarkClient);
        }

        public virtual ISlarkMessage CreateMessage(string message)
        {
            return new SlarkStandardMessage()
            {
                MetaText = message
            };
        }

        public override Task<string> OnRPC(string method, params object[] rpcParamters)
        {
            return this.RPCAllAsync(method, rpcParamters);
        }

        public virtual Task<ISlarkProtocol> MatchProtocolAsync(SlarkContext context)
        {
            if (ProtocolMatcher == null)
                return Task.FromResult(new EchoProtocol() as ISlarkProtocol);
            return ProtocolMatcher.FindAsync(context);
        }

        public override void AddConnectionSync(SlarkClientConnection connection)
        {
            lock (ConnectionList)
            {
                ConnectionList.Add(connection);
            }
        }

        public override void RemoveConnectionSync(SlarkClientConnection connection)
        {
            lock (ConnectionList)
            {
                ConnectionList.Remove(connection);
            }
        }
    }
}
