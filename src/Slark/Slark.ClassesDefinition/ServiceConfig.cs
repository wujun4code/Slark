using System;
using System.Collections.Generic;

namespace Slark.ClassesDefinition
{
    public struct SlarkServiceConfig
    {
        public IDictionary<HostType, UInt16> Hosts { get; set; }

        public SlarkLobby.Config LobbyConfig { get; set; }

        public SlarkRoom.Config RoomConfig { get; set; }

        public static SlarkServiceConfig Default
        {
            get
            {
                return new SlarkServiceConfig()
                {
                    Hosts = new Dictionary<HostType, UInt16>()
                    {
                        { HostType.Lobby, 1},
                    },
                    LobbyConfig = new SlarkLobby.Config()
                    {
                        MaxRoomCount = 30
                    },
                    RoomConfig = new SlarkRoom.Config()
                    {
                        MaxPeerCount = 10
                    },
                };
            }
        }
    }
}
