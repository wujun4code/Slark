using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public abstract class PlayServer : SlarkStandardServer
    {
        public override SlarkContext CreateContext(SlarkClientConnection slarkClientConnection, string message)
        {
            var coreContext = base.CreateContext(slarkClientConnection, message);
            return new PlayContext(coreContext);
        }

        public override ISlarkMessage CreateMessage(string message)
        {
            return new PlayRequest(message);
        }

        public override Task<SlarkClient> CreateAsync(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(new StandardPlayClient() as SlarkClient);
        }
    }
}
