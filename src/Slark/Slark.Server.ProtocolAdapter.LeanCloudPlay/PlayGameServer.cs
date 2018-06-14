﻿using Slark.Core;
using Slark.Core.Utils;
using Slark.Server.LeanCloud.Play.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
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
            protocolMatcher.AddCommandHandler(new RoomUpdate());
            protocolMatcher.AddCommandHandler(new RoomJoin());

            ProtocolMatcher = protocolMatcher;

            Rooms = new ConcurrentHashSet<PlayRoom>();
        }

        public PlayLobbyServer LobbyServer { get; set; }

        public ConcurrentHashSet<PlayRoom> Rooms { get; set; }


        public Task<PlayRoom> CreateEmptyRoomAsync(RoomConfig config)
        {
            var room = new PlayRoom(config);
            Rooms.Add(room);
            return Task.FromResult(room);
        }

        public Task<PlayRoom> CreateWithConfigAsync(RoomConfig config, SlarkClientConnection connection)
        {
            var player = new Player()
            {
                ClientConnection = connection,
                ActorId = 1,
            };

            var room = new PlayRoom(config)
            {
                MasterClientId = player.Client.PeerId,
                CreatorId = player.Client.PeerId,
                Players = new ConcurrentHashSet<Player>()
                {
                    { player }
                }
            };

            player.Client.RoomId = room.Id;

            Rooms.Add(room);

            return Task.FromResult(room);
        }

        public override Task OnConnected(SlarkClientConnection slarkClientConnection)
        {
            slarkClientConnection.Client = new PlayClient();
            return Task.FromResult(true);
        }

        public async override Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            if (slarkClientConnection.Client is PlayClient playClient)
            {
                var noticeBody = new Dictionary<string, object>()
                {
                    { "cmd", "conv" },
                    { "op", "members-offline" },
                };

                noticeBody["initBy"] = playClient.PeerId;

                var room = await FindRoomByClientAsync(playClient);
                if (room != null)
                {
                    noticeBody["cid"] = room.Id;
                    var player = await room.FindPlayerByClient(playClient);
                    if (player != null)
                    {
                        noticeBody["initByActor"] = player.ActorId;
                        noticeBody["master"] = player.PeerId == room.MasterClientId;
                    }
                    var notice = new PlayNotice()
                    {
                        Body = noticeBody
                    };
                    notice.Timestamplize();
                    notice.SerializeBody();
                    await room.BroadcastAsync(notice);
                }
            }
            await base.OnDisconnected(slarkClientConnection);
        }

        public Task<PlayRoom> FindRoomByClientAsync(PlayClient client)
        {
            if (string.IsNullOrEmpty(client.RoomId)) return Task.FromResult<PlayRoom>(null);
            var room = Rooms.FirstOrDefault(r => r.Id == client.RoomId);
            return Task.FromResult<PlayRoom>(room);
        }

        public Task<PlayRoom> FindRoomMatchRequest(RoomJoinRequest joinRequest)
        {
            var room = Rooms.FirstOrDefault(r =>
            {
                var matched = false;

                if (!string.IsNullOrEmpty(joinRequest.Name))
                {
                    matched = r.Id == joinRequest.Name;
                }

                if (joinRequest.ExpectedUsers != null && r.ExpectedMemberPeerIds != null)
                {
                    matched = r.ExpectedMemberPeerIds.SequenceEqual(joinRequest.ExpectedUsers);
                }

                if (joinRequest.ExpectAttributes != null && r.CustomRoomProperties != null)
                {
                    matched = r.CustomRoomProperties.ValueEquals(joinRequest.ExpectAttributes);
                }
                return matched;
            });

            return Task.FromResult(room);
        }
    }
}