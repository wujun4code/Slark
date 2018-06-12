using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public abstract class PlayCommandHandlerBase : IPlayCommandHandler
    {
        public virtual string Command { get; set; }
        public virtual string Operation { get; set; }

        public virtual Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            return Task.FromResult(context.Server.Connections as IEnumerable<SlarkClientConnection>);
        }

        public virtual Task<string> NotifyAsync(SlarkContext context)
        {
            return Task.FromResult(context.Message.MetaText);
        }

        public virtual Task<string> ResponseAsync(SlarkContext context)
        {
            var request = new PlayRequest(context.Message.MetaText);
            return ResponseAsync(request, context);
        }

        public virtual Task<string> ResponseAsync(PlayRequest request, SlarkContext context)
        {
            return request.Body.ToJsonStringAsync();
        }
    }
}
