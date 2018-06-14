using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public interface IPlayRPCHandler
    {
        string Method { get; set; }

        Task<string> RPCAsync(SlarkServer server, params object[] rpcParamters);
    }
}
