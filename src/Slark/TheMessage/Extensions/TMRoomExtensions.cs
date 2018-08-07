using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheMessage.Extensions
{
    public static class TMRoomExtensions
    {
        public static Task RpcAllAsync(this TMRoom @this, string methodName, params object[] args)
        {
            var rpcMethodName = $"TMRoom_{@this.Id}_{methodName}";
            var sendTasks = @this.ClientInfos.Select(info => info.Client.RpcAsync(rpcMethodName, args));
            return Task.WhenAll(sendTasks);
        }

        public static Task RpcClientAsync(this TMRoom @this, TMClient client, string methodName, params object[] args)
        {
            var rpcMethodName = $"TMRoom_{@this.Id}_{methodName}";
            return client.RpcAsync(rpcMethodName, args);
        }
    }
}
