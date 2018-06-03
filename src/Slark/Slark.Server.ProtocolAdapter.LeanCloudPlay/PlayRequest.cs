using Slark.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayRequest
    {
        public PlayRequest(string jsonMessage)
        {
            MetaString = jsonMessage;
            Body = jsonMessage.ToDictionary();
        }

        public string MetaString { get; set; }

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


    }
}
