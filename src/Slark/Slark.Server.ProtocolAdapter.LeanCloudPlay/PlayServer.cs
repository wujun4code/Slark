using System;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayServer : SlarkStandardServer
    {
        public PlayServer()
        {
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
