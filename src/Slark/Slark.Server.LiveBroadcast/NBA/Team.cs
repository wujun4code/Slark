using System;
using Newtonsoft.Json;

namespace Slark.Server.LiveBroadcast.NBA
{
    public class Team
    {
        public Team()
        {
        }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        [JsonProperty("triCode")]
        public string TribleCode { get; set; }

        [JsonProperty("score")]
        public uint Score { get; set; }
    }
}
