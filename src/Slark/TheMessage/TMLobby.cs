using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Extensions;
using Slark.Server.LeanCloud.Play;

namespace TheMessage
{
    public class TMLobby : PlayLobbyServer
    {
        public TMLobby()
        {

        }

        public TMLobby(IEnumerable<PlayGameServer> gameServers = null)
            : base(gameServers)
        {

        }

        [Injective("CreateClientAsync")]
        public Task<SlarkClient> Create(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(new TMClient() as SlarkClient);
        }
    }
}
