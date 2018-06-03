using System;
using System.Collections.Generic;
using System.Text;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayContext
    {
        public PlayServer Server { get; set; }

        public PlayRequest Request { get; set; }

        public PlayResponse Response { get; set; }
    }
}
