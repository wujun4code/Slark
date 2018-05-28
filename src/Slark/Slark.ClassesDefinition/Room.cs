using System;
using System.Collections.Generic;

namespace Slark.ClassesDefinition
{
    public class SlarkRoom : SlarkRole
    {
        public SlarkRoom()
        {
        }

        public struct Config
        {
            public UInt16 MaxPeerCount { get; set; }
        }

        public IEnumerable<SlarkConnection<SlarkPeer>> PeerConnections { get; set; }
    }
}
