using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanCloud;
using Slark.Core;
using TheMessage.Extensions;

namespace TheMessage
{
    public static class TM
    {
        public static void Init()
        {
            SetupPlugins();
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

        public static void SetupPlugins()
        {
            SlarkCorePlugins.Singleton.Decoder = new LeanCloudJsonDecoder();
            SlarkCorePlugins.Singleton.Encoder = new LeanCloudJsonEncoder();
        }
    }
}
