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
    public class StandardLobbyServer : PlayLobbyServer
    {
        public StandardLobbyServer(IEnumerable<PlayGameServer> gameServers = null)
            : base(gameServers)
        {

        }
    }

    public abstract class PlayLobbyServer : PlayServer
    {
        public IDictionary<string, IPlayRPCHandler> RPCHandlers { get; set; }

        public ConcurrentDictionary<string, PlayClient> TokenClientMap { get; set; }

        public List<string> GameServerUrls { get; set; }

        public IEnumerable<PlayGameServer> GameServers { get; set; }

        public PlayLobbyServer(IEnumerable<PlayGameServer> gameServers = null)
        {
            if (gameServers != null)
            {
                GameServerUrls = gameServers.Select(gs => gs.ClientConnectionUrl).ToList();
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
            protocolMatcher.AddCommandHandler(new RoomJoin());
            protocolMatcher.AddCommandHandler(new RoomRandomJoin());

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
                return RPCHandlers[method].RPCAsync(this, rpcParamters);
            }
            return this.OnRPC(method, rpcParamters);
        }

        public Task<PlayClient> FindClientAsync(string token)
        {
            TokenClientMap.TryGetValue(token, out PlayClient client);
            if (client == null)
            {
                client = new StandardPlayClient();
            }
            return Task.FromResult(client);
        }

        public async Task<Tuple<PlayGameServer, PlayRoom>> MatchAsync(RoomJoinRequest joinRequest)
        {
            foreach (var gameServer in GameServers)
            {
                var room = await gameServer.FindRoomMatchRequest(joinRequest);
                if (room != null)
                    return new Tuple<PlayGameServer, PlayRoom>(gameServer, room);
            }

            var createIfNotFound = joinRequest.CreateIfNotFound.HasValue && joinRequest.CreateIfNotFound.Value;

            if (createIfNotFound)
            {
                var randomGameServer = GameServers.RandomOne();

                var room = await randomGameServer.CreateEmptyRoomAsync(new RoomConfig()
                {
                    Name = joinRequest.Name
                });

                return new Tuple<PlayGameServer, PlayRoom>(randomGameServer, room);
            }

            return await RandomMatchAsync();
        }

        public Task<Tuple<PlayGameServer, PlayRoom>> RandomMatchAsync()
        {
            var randomGameServer = GameServers.RandomOne();
            var randomRoom = randomGameServer.Rooms.RandomOne();
            return Task.FromResult(new Tuple<PlayGameServer, PlayRoom>(randomGameServer, randomRoom));
        }
    }
}
