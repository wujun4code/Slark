using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slark.Core;
using Slark.Server.LeanCloud.Play.Protocol;
using System.Collections.Concurrent;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayLobbyServer : SlarkStandardServer
    {
        public IDictionary<string, IPlayRPCHandler> RPCHandlers { get; set; }

        public ConcurrentDictionary<string, PlayClient> TokenClientMap { get; set; }

        public List<string> GameServerUrls { get; set; }

        public IEnumerable<PlayGameServer> GameServers { get; set; }

        public PlayLobbyServer(IEnumerable<PlayGameServer> gameServers = null)
        {
            if (gameServers != null)
            {
                GameServerUrls = gameServers.Select(gs => gs.ServerUrl).ToList();
                GameServers = gameServers;
                foreach (var gameServer in gameServers)
                {
                    gameServer.LobbyServer = this;
                }
            }
            else
            {
                GameServerUrls = new List<string>();
                GameServers = new List<PlayGameServer>();
            }

            RPCHandlers = new Dictionary<string, IPlayRPCHandler>();
            AddRPCHandler(new LobbyRouterHandler());

            var protocolMatcher = new PlayProtocolMatcher();
            protocolMatcher.AddCommandHandler(new SessionOpen());
            protocolMatcher.AddCommandHandler(new RoomStart());

            ProtocolMatcher = protocolMatcher;

            TokenClientMap = new ConcurrentDictionary<string, PlayClient>();
        }

        public void AddRPCHandler(IPlayRPCHandler playRouteHandler)
        {
            RPCHandlers.Add(playRouteHandler.Method, playRouteHandler);
        }

        public override Task<string> OnRPC(string method, params object[] rpcParamters)
        {
            if (RPCHandlers.ContainsKey(method))
            {
                return RPCHandlers[method].RPC(rpcParamters);
            }
            return this.OnRPC(method, rpcParamters);
        }

        public override Task OnConnected(SlarkClientConnection slarkClientConnection)
        {
            slarkClientConnection.Client = new PlayClient();
            return Task.FromResult(true);
        }

        public Task<PlayClient> FindClientAsync(string token)
        {
            TokenClientMap.TryGetValue(token, out PlayClient client);
            if (client == null)
            {
                client = new PlayClient();
            }
            return Task.FromResult(client);
        }
    }
}
