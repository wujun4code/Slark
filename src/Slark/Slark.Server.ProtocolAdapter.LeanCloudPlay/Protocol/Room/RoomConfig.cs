using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomConfig
    {
        public RoomConfig()
        {

        }

        /// <summary>
        /// set a custom properties for room before create it.
        /// </summary>
        [JsonProperty("cid")]
        public string Name { get; set; }

        /// <summary>
        /// set a custom properties for room before create it.
        /// </summary>
        [JsonProperty("attr")]
        public Hashtable CustomRoomProperties { get; set; }

        /// <summary>
        /// the keys in CustomProperties to match a room in lobby.
        /// </summary>
        [JsonProperty("lobbyAttrKeys")]
        public IEnumerable<string> LobbyMatchKeys { get; set; }

        /// <summary>
        /// expected user userId(s)
        /// </summary>
        [JsonProperty("expectMembers")]
        public IEnumerable<string> ExpectedUsers { get; set; }

        /// <summary>
        /// max player count in a Room
        /// </summary>
        [JsonProperty("maxMembers")]
        public byte? MaxPlayerCount { get; set; }

        /// <summary>
        /// visible in lobby
        /// </summary>
        [JsonProperty("visible")]
        public bool? IsVisible { get; set; }

        /// <summary>
        /// open to join and find
        /// </summary>
        [JsonProperty("open")]
        public bool? IsOpen { get; set; }

        /// <summary>
        /// empty time to live for the Room in seconds. max value is 1800(seconds).
        /// </summary>
        [JsonProperty("emptyRoomTtl")]
        public uint? EmptyTimeToLive { get; set; }

        /// <summary>
        /// time(unit:second) to keep a player when disconnect from room. max value is 60(seconds).
        /// </summary>
        [JsonProperty("playerTtl")]
        public uint? PlayerTimeToKeep { get; set; }

    }
}
