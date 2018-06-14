using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomJoinRequest
    {
        [JsonProperty("cid")]
        public string Name { get; set; }

        [JsonProperty("expectMembers")]
        public IEnumerable<string> ExpectedUsers { get; set; }

        [JsonProperty("createOnNotFound")]
        public bool? CreateIfNotFound { get; set; }

        [JsonProperty("rejoin")]
        public bool? IsRejoin { get; set; }

        [JsonProperty("expectAttr")]
        public Hashtable ExpectAttributes { get; set; }
    }
}
