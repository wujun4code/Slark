using System;
using System.Linq;
using System.Threading.Tasks;

namespace TheMessage.Extensions
{
    public static class TMGameExtensions
    {
        public static Task RpcAllAsync(this TMGame @this, string methodName, params object[] args)
        {
            var room = @this.Room;
            var rpcMethodName = $"TMGame_{@this.Id}_{methodName}";
            var sendTasks = room.ClientInfos.Select(info => info.Client.RpcAsync(rpcMethodName, args));
            return Task.WhenAll(sendTasks);
        }

        public static Task RpcPlayerAsync(this TMGame @this, TMPlayer player, string methodName, params object[] args)
        {
            var room = @this.Room;
            var rpcMethodName = $"TMGame_{@this.Id}_{methodName}";
            return player.Client.RpcAsync(rpcMethodName, args);
        }
    }
}
