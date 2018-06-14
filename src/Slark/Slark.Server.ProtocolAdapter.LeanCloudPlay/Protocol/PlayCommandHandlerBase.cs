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

        public Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            var request = new PlayRequest(context.Message.MetaText);
            return GetTargetsAsync(request, context);
        }

        public virtual Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(PlayRequest request, SlarkContext context)
        {
            var receivers = context.Receivers ?? context.Sender.ToEnumerable();
            return Task.FromResult(context.Receivers);
        }

        public async Task<string> NotifyAsync(SlarkContext context)
        {
            var request = new PlayRequest(context.Message.MetaText);
            var notice = await NotifyAsync(request, context);
            return notice.MetaText;
        }

        public virtual Task<PlayNotice> NotifyAsync(PlayRequest request, SlarkContext context)
        {
            var notice = context.Notice ?? request.MetaText;
            return Task.FromResult(new PlayNotice()
            {
                MetaText = notice
            });
        }

        public async Task<string> ResponseAsync(SlarkContext context)
        {
            var request = new PlayRequest(context.Message.MetaText);
            var response = await ResponseAsync(request, context);
            return response.MetaText;
        }

        public virtual Task<PlayResponse> ResponseAsync(PlayRequest request, SlarkContext context)
        {
            var response = context.Response ?? request.MetaText;
            return Task.FromResult(new PlayResponse()
            {
                MetaText = response
            });
        }
    }
}
