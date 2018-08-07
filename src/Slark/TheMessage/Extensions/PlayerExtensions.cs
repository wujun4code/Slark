using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slark.Core;

namespace TheMessage.Extensions
{
    public static class PlayerExtensions
    {
        public static Task AlloctAsync(this TMPlayer player, IEnumerable<TMCharacter> characters)
        {
            var text = SlarkCorePlugins.Singleton.Encoder.Encode(characters);
            return player.Client.SendAsync(text);
        }

        public static Task RpcAsync(this TMPlayer @this, string methodName, params object[] args)
        {
            var rpcMethodName = $"TMPlayer_{@this.Id}_{methodName}";
            return @this.Client.RpcAsync(rpcMethodName, args);
        }

        public static Task<T> RpcWithChoiceAsync<T>(this TMPlayer @this, string methodName, IEnumerable<T> choices, bool canBeSkip = false, int timeLimitForDefaultResult = 15000)
        {
            var rpcMethodName = $"TMPlayer_{@this.Id}_{methodName}";
            return @this.Client.RpcWithChoiceAsync<T>(rpcMethodName, choices, canBeSkip, timeLimitForDefaultResult);
        }
    }
}
