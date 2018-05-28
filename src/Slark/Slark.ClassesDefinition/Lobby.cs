using System;
using System.Collections.Generic;

namespace Slark.ClassesDefinition
{
    public class SlarkLobby : SlarkRole
    {
        void Handle_OnAttached(SlarkNode obj)
        {
            var peerConnection = new SlarkConnection<SlarkPeer>()
            {
                TheOtherSide = new SlarkPeer()
                {

                }
            };

        }

        public struct Config
        {
            public UInt16 MaxRoomCount { get; set; }
        }


        public IEnumerable<SlarkConnection<SlarkPeer>> PeerConnections { get; set; }

        public IEnumerable<SlarkConnection<SlarkRoom>> RoomConnections { get; set; }


        public void Start()
        {

            this.OnAttached += Handle_OnAttached;
        }
    }
}
