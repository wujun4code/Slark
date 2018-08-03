using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheMessage.Extensions
{
    public static class TMClientExtensions
    {
        public static Task RpcAsync(this IEnumerable<TMClient> @this, string methodName, params object[] args)
        {
            var sendTasks = @this.Select(client => client.RpcAsync(methodName, args));
            return Task.WhenAll(sendTasks);
        }

        public static Task BroadcastAsync(this IEnumerable<TMClient> @this,string message)
        {
            var sendTasks = @this.Select(client => client.SendAsync(message));
            return Task.WhenAll(sendTasks);
        }
    }
}
