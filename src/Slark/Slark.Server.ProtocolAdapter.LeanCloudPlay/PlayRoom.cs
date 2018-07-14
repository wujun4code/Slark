using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Slark.Core.Utils;
using Slark.Server.LeanCloud.Play.Protocol;
using System.Collections;
using System.Threading.Tasks;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play
{

    public class StandardPlayRoom : PlayRoom
    {
        public StandardPlayRoom(RoomConfig config) : base(config)
        {

        }
    }

    public abstract class PlayRoom
    {
        public PlayRoom(RoomConfig config)
        {
            Config = config;

            Name = config.Name;
            IsVisible = !config.IsVisible.HasValue || config.IsVisible.Value;
            IsOpen = !config.IsOpen.HasValue || config.IsOpen.Value;
            EmptyTimeToLive = config.EmptyTimeToLive ?? 3600;
            PlayerTimeToKeep = config.PlayerTimeToKeep ?? 600;
            ExpectedMemberPeerIds = config.ExpectedUsers;
            MaxPlayerCount = config.MaxPlayerCount ?? (byte)10;

            CustomRoomProperties = config.CustomRoomProperties ?? new Hashtable();
        }

        public Hashtable CustomRoomProperties { get; set; }

        public Player MasterClient
        {
            get
            {
                return Players.FirstOrDefault(c => c.PeerId == MasterClientId);
            }
        }

        public Player Creator
        {
            get
            {
                return Players.FirstOrDefault(c => c.PeerId == CreatorId);
            }
        }

        public uint CurrentMaxActorId
        {
            get => Players.Max(p => p.ActorId);
        }

        public ConcurrentHashSet<Player> Players { get; set; }

        public RoomConfig Config
        {
            get; set;
        }

        public string Id
        {
            get
            {
                return Name;
            }
        }

        public string Name
        {
            get; set;
        }

        public bool IsVisible { get; set; }

        public bool IsOpen { get; set; }

        public uint EmptyTimeToLive { get; set; }

        public uint PlayerTimeToKeep { get; set; }

        public uint TimeToKeep { get; set; }

        public IEnumerable<string> ExpectedMemberPeerIds { get; set; }

        public byte MaxPlayerCount { get; set; }

        public string MasterClientId
        {
            get; set;
        }

        public uint MasterActorId
        {
            get
            {
                return MasterClient.ActorId;
            }
        }

        public string CreatorId
        {
            get; set;
        }

        public IEnumerable<string> MemberIds
        {
            get
            {
                return Players.Select(c => c.PeerId);
            }
        }

        public IEnumerable<uint> ActorIds
        {
            get
            {
                return Players.Select(p => p.ActorId);
            }
        }

        public IEnumerable<IDictionary<string, object>> MembersJsonFormatting
        {
            get
            {
                return Players.Select(p => p.ToJsonDictionary());
            }
        }

        public IEnumerable<SlarkClientConnection> AvailableConnections
        {
            get => this.Players.Select(p => p.ClientConnection);
        }

        public Task<Player> FindPlayerByClient(PlayClient client)
        {
            return Task.FromResult(Players.FirstOrDefault(p => p.Client == client));
        }

        public Task BroadcastAsync(PlayNotice notice)
        {
            var noticeText = notice.MetaText;
            var connections = Players.Select(p => p.ClientConnection);
            return connections.BroadcastAsync(noticeText);
        }

        public Task RPCAsync(string methodName, params object[] methodParameters)
        {
            var notice = new PlayNotice()
            {
                Body = new Dictionary<string, object>()
            };

            var msg = new Dictionary<string, object>
            {
                { "m_n", methodName },
                { "m_p", methodParameters }
            };

            notice.Body["cmd"] = "direct";
            notice.Body["cid"] = this.Id;
            notice.Body["msg"] = msg.ToJsonString();

            return BroadcastAsync(notice);
        }

        public Task<Tuple<PlayResponse, PlayNotice>> NewPlayerJoinAsync(PlayRequest request, SlarkClientConnection connection)
        {
            var newPlayer = new StandardPlayer()
            {
                ActorId = this.CurrentMaxActorId + 1,
                ClientConnection = connection
            };

            this.Players.Add(newPlayer);

            var responseBody = new Dictionary<string, object>()
                {
                    { "cmd","conv" },
                    { "op", "added" },
                    { "i", request.CommandId },
                };

            var open = this.Players.Count < this.MaxPlayerCount;
            responseBody["open"] = open;

            var visible = this.IsVisible;
            responseBody["visible"] = visible;

            responseBody["m"] = this.MemberIds;
            responseBody["memberIds"] = this.ActorIds;
            responseBody["actorIds"] = this.ActorIds;
            responseBody["masterClientId"] = this.MasterClientId;

            responseBody["masterActorId"] = this.MasterActorId;

            responseBody["expectMembers"] = this.ExpectedMemberPeerIds;

            responseBody["playerTtl"] = this.PlayerTimeToKeep;
            responseBody["emptyRoomTtl"] = this.EmptyTimeToLive;
            responseBody["maxMembers"] = this.MaxPlayerCount;
            responseBody["members"] = this.MembersJsonFormatting;
            responseBody["ttlSecs"] = this.TimeToKeep;
            responseBody["attr"] = this.CustomRoomProperties;

            var response = new PlayResponse(responseBody);

            response.Timestamplize();
            response.SerializeBody();


            var noticeBody = new Dictionary<string, object>()
                {
                    { "cmd","conv" },
                    { "op", "members-joined" },
                };

            noticeBody["initBy"] = newPlayer.PeerId;
            noticeBody["master"] = newPlayer.PeerId == this.MasterClientId;
            noticeBody["memberId"] = newPlayer.ActorId;
            noticeBody["initByActor"] = newPlayer.ActorId;

            var notice = new PlayNotice(noticeBody);

            notice.Timestamplize();
            notice.SerializeBody();

            return Task.FromResult(new Tuple<PlayResponse, PlayNotice>(response, notice));
        }
    }
}
