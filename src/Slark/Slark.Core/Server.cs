using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace Slark.Core
{
    public abstract class SlarkServer
    {
        public SlarkServer()
        {

        }

        public abstract Task OnConnected(SlarkClientConnection slarkClientConnection);

        public abstract Task OnReceived(SlarkClientConnection slarkClientConnection, string message);

        public abstract Task OnDisconnected(SlarkClientConnection slarkClientConnection);

        public abstract Task<string> OnRPC(string route, string message);

        public abstract List<SlarkClientConnection> Connections { get; set; }

        public virtual Task Broadcast(string message)
        {
            var sendTasks = Connections.Select(connection => connection.SendAsync(message));
            return Task.WhenAll(sendTasks);
        }
    }
}
