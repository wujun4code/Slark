using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class PlayProtocolMatcher : SlarkStandardProtocolMatcher
    {
        public IDictionary<string, IPlayCommandHandler> CommandHandlers { get; set; }

        public void AddCommandHandler(IPlayCommandHandler playCommandHandler, string fixKey = null)
        {
            if (CommandHandlers == null) CommandHandlers = new Dictionary<string, IPlayCommandHandler>();
            var key = !string.IsNullOrEmpty(fixKey) ? fixKey : playCommandHandler.Command + "-" + playCommandHandler.Operation;
            CommandHandlers[key] = playCommandHandler;
        }

        public override Task<ISlarkProtocol> FindAsync(SlarkContext context)
        {
            var request = new PlayRequest(context.Message.MetaText);
            if (request.IsValid)
            {
                if (CommandHandlers.ContainsKey(request.CommandHandlerKey))
                {
                    return Task.FromResult(CommandHandlers[request.CommandHandlerKey] as ISlarkProtocol);
                }
            }

            return base.FindAsync(context);
        }
    }
}
