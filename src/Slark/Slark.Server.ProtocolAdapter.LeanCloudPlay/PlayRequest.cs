using Slark.Core;
using Slark.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayRequest : ISlarkMessage
    {
        public PlayRequest(string jsonMessage)
        {
            MetaText = jsonMessage;
            Body = jsonMessage.ToDictionary();
        }

        public string MetaText { get; set; }

        public IDictionary<string, object> Body { get; set; }

        public string CommandHandlerKey
        {
            get
            {
                return Body["cmd"] + "-" + Body["op"];
            }
        }

        public int CommandId
        {
            get
            {
                return int.Parse(Body["i"].ToString());
            }
        }

        public bool IsValid
        {
            get
            {
                return Body.ContainsKey("cmd") && Body.ContainsKey("op");
            }
        }

    }
}
