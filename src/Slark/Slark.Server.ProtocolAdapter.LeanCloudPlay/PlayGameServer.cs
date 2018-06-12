using Slark.Core;
using Slark.Core.Utils;
using Slark.Server.LeanCloud.Play.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayGameServer : SlarkStandardServer
    {
        public PlayGameServer()
        {
            var protocolMatcher = new PlayProtocolMatcher();
            protocolMatcher.AddCommandHandler(new RoomCreate());
            protocolMatcher.AddCommandHandler(new SessionOpen());

            ProtocolMatcher = protocolMatcher;

            Rooms = new ConcurrentHashSet<PlayRoom>();
        }

        public PlayLobbyServer LobbyServer { get; set; }

        public ConcurrentHashSet<PlayRoom> Rooms { get; set; }

        public Task<PlayRoom> CreateWithConfigAsync(RoomConfig config, PlayClient creator)
        {
            var player = new Player()
            {
                Client = creator,
                ActorId = 1,
            };
            var room = new PlayRoom(config)
            {
                MasterClientId = creator.PeerId,
                CreatorId = creator.PeerId,
                Players = new ConcurrentHashSet<Player>()
                {
                    { player }
                }
            };
            Rooms.Add(room);
            return Task.FromResult(room);
        }

        public override Task OnConnected(SlarkClientConnection slarkClientConnection)
        {
            slarkClientConnection.Client = new PlayClient();
            return Task.FromResult(true);
        }
    }
}
