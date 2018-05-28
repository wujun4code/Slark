using System;
using System.Collections.Generic;

namespace Slark.ClassesDefinition
{
    public class SlarkRoute : SlarkRole
    {
        public IEnumerable<SlarkConnection<SlarkLobby>> LobbyConnections { get; set; }
    }
}
