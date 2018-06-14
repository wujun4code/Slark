using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Concurrent;

namespace Slark.Core
{
    public abstract class SlarkServer
    {
        public abstract Task OnConnected(SlarkClientConnection slarkClientConnection);

        public abstract Task OnReceived(SlarkClientConnection slarkClientConnection, string message);

        public abstract Task OnDisconnected(SlarkClientConnection slarkClientConnection);

        public abstract Task<string> OnRPC(string method, params object[] rpcParamters);

        public virtual Task BroadcastAsync(string message)
        {
            var sendTasks = Connections.Select(connection => connection.SendAsync(message));
            return Task.WhenAll(sendTasks);
        }

        public abstract IEnumerable<SlarkClientConnection> Connections { get; }

        public abstract string ServerUrl { get; set; }

        public abstract string ClientConnectingAddress { get; set; }

        public abstract void AddConnectionSync(SlarkClientConnection connection);

        public abstract void RemoveConnectionSync(SlarkClientConnection connection);

        public virtual async Task<string> RPCAllAsync(string method, params object[] rpcParamters)
        {
            var message = new Dictionary<string, object>()
            {
                { "method", method },
                { "params", rpcParamters }
            }.ToJsonString();

            await BroadcastAsync(message);
            return message;
        }
    }
}
