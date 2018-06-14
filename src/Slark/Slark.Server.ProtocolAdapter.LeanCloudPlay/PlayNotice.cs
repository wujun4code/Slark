using System;
using System.Collections.Generic;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayNotice : PlayJsonMessage
    {
        public PlayNotice()
        {

        }

        public PlayNotice(IDictionary<string, object> json)
            : base(json)
        {

        }
    }
}
