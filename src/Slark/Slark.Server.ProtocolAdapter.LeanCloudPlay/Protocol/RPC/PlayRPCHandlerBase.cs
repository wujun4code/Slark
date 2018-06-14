using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public abstract class PlayRPCHandlerBase : IPlayRPCHandler
    {
        public abstract string Method { get; set; }

        public virtual Task<string> RPCAsync(SlarkServer server, params object[] rpcParamters)
        {
            return Task.FromResult("");
        }
    }
}
