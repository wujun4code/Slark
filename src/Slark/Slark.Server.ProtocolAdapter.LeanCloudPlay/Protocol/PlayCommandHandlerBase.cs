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
            var receivers = context.Receivers ?? context.Sender.ToEnumerable();
            return Task.FromResult(receivers);
        }

        public virtual Task<string> NotifyAsync(SlarkContext context)
        {
            var notice = context.Notice ?? context.Message.MetaText;
            return Task.FromResult(notice);
        }


        public virtual Task<string> ResponseAsync(SlarkContext context)
        {
            var response = context.Response ?? context.Message.MetaText;
            return Task.FromResult(response);
        }
    }
}
