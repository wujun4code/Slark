using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slark.Core;
using Slark.Core.Protocol;

namespace TheMessage
{
    public class TMJsonRequest : ISlarkMessage
    {
        public TMJsonRequest()
        {

        }

        public TMJsonRequest(string metaText)
        {
            MetaText = metaText;
        }

        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("headers")]
        public IList<KeyValuePair<string, string>> Headers { get; set; }
        [JsonProperty("body")]
        public IDictionary<string, object> Body { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("i")]
        public int CommandId { get; set; }

        public string MetaText { get; set; }

    }
}
