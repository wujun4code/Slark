using System;
using Slark.Core.Protocol;
using System.Collections.Generic;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayResponse : PlayJsonMessage
    {
        public PlayResponse()
        {

        }

        public PlayResponse(IDictionary<string, object> json)
            : base(json)
        {

        }
    }
}
