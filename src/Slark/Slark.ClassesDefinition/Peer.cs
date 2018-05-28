using System;

namespace Slark.ClassesDefinition
{
    public class SlarkPeer : SlarkRole
    {
        public SlarkConnection<SlarkLobby> LobbyConnection { get; set; }

        public SlarkConnection<SlarkRoom> RoomConnection { get; set; }
    }
}
