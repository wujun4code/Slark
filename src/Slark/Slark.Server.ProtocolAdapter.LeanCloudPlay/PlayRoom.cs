using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Slark.Core.Utils;
using Slark.Server.LeanCloud.Play.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayRoom
    {
        public PlayRoom(RoomConfig config)
        {
            Config = config;

            Name = config.Name;
            IsVisible = config.IsVisible.HasValue ? config.IsVisible.Value : true;
            IsOpen = config.IsOpen.HasValue ? config.IsOpen.Value : true;
            EmptyTimeToLive = config.EmptyTimeToLive.HasValue ? config.EmptyTimeToLive.Value : 3600;
            PlayerTimeToKeep = config.PlayerTimeToKeep.HasValue ? config.PlayerTimeToKeep.Value : 600;
            ExpectedMemberPeerIds = config.ExpectedUsers;
            MaxPlayerCount = config.MaxPlayerCount.HasValue ? config.MaxPlayerCount.Value : (byte)10;
        }

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
    }
}
