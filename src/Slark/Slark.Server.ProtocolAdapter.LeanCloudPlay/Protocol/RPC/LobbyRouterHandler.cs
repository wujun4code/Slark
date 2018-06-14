using Slark.Core;
using Slark.Server.LeanCloud.Play.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class LobbyRouterHandler : PlayRPCHandlerBase
    {
        public override string Method { get; set; } = "router";

        public override Task<string> RPCAsync(SlarkServer server, params object[] rpcParamters)
        {
            var lobbyRouterResponse = new Dictionary<string, object>()
            {
                { "server", "ws://localhost:5000/lobby" },
                { "secondary", "ws://localhost:5000/lobby" },
                { "ttl", 1440 }
            };

            if (server is PlayLobbyServer lobby)
            {
                lobbyRouterResponse["server"] = lobby.ClientConnectingAddress;
                lobbyRouterResponse["secondary"] = lobby.ClientConnectingAddress;
            }

            return Task.FromResult(lobbyRouterResponse.ToJsonString());
        }
    }
}
