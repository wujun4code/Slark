using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public abstract class PlayServer : SlarkStandardServer
    {
        public PlayServer()
        {
            this.CreateClientAsync = (connection) =>
            {
                return Task.FromResult(new StandardPlayClient() { } as SlarkClient);
            };
        }

        public override SlarkContext CreateContext(SlarkClientConnection slarkClientConnection, string message)
        {
            var coreContext = base.CreateContext(slarkClientConnection, message);
            return new PlayContext(coreContext);
        }

        public override ISlarkMessage CreateMessage(string message)
        {
            return new PlayRequest(message);
        }
    }
}
