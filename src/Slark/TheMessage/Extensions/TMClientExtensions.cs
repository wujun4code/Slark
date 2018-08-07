using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeanCloud.Core.Internal;
using LeanCloud.Utilities;
using Slark.Core.Extensions;

namespace TheMessage.Extensions
{
    public static class TMClientExtensions
    {
        public static Task RpcAsync(this IEnumerable<TMClient> @this, string methodName, params object[] args)
        {
            var sendTasks = @this.Select(client => client.RpcAsync(methodName, args));
            return Task.WhenAll(sendTasks);
        }

        public static async Task<IDictionary<string, object>> RpcAsync(this TMClient @this, string methodName, params object[] args)
        {
            var rpcRequest = new TMJsonRequest();

            var encodedArgs = new List<object>();

            foreach (var a in args)
            {
                var ea = TMEncoding.Instance.Encode(a);
                encodedArgs.Add(ea);
            }

            Dictionary<string, object> body = new Dictionary<string, object>()
            {
                { "args", encodedArgs},
            };

            rpcRequest.Url = string.Format("/rpc/{0}", methodName);
            rpcRequest.Body = body;
            var json = await @this.SendAsync(rpcRequest);
            var result = json.Results;
            return result;
        }

        public static async Task<T> RpcWithChoiceAsync<T>(this TMClient @this, string methodName, IEnumerable<T> choices, bool canBeSkip = false, int timeLimitForDefaultResult = 15000)
        {
            var rpcRequest = new TMJsonRequest();

            var encodedArgs = new List<object>();
            object[] args = new object[] { choices, canBeSkip, timeLimitForDefaultResult };
            foreach (var a in args)
            {
                var ea = TMEncoding.Instance.Encode(a);
                encodedArgs.Add(ea);
            }

            Dictionary<string, object> body = new Dictionary<string, object>()
            {
                { "args", encodedArgs},
            };

            rpcRequest.Url = string.Format("/rpc/{0}", methodName);
            rpcRequest.Body = body;
            if (timeLimitForDefaultResult == 0)
            {
                timeLimitForDefaultResult = 15000;
            }
            var json = await @this.SendAsync(rpcRequest, true, timeLimitForDefaultResult);
            var result = json.Results;
            if (result == null)
            {
                if (!canBeSkip)
                {
                    return choices.PickRandom();
                }
            }
            var deocdedObj = AVDecoder.Instance.Decode(result);
            T rtn = Conversion.To<T>(deocdedObj);

            return rtn;
        }

        public static Task BroadcastAsync(this IEnumerable<TMClient> @this, string message)
        {
            var sendTasks = @this.Select(client => client.SendAsync(message));
            return Task.WhenAll(sendTasks);
        }
    }
}
