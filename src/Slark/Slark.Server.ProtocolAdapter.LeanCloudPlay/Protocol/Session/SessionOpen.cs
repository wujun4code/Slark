using Slark.Core;
using Slark.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class SessionOpen : SlarkStandardProtocolMatcher, IPlayCommandHandler
    {
        public string Command { get; set; } = "session";
        public string Operation { get; set; } = "open";

        public Task ExecuteAsync(PlayContext context)
        {
            context.Response.Body = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "opened"},
                { "i", context.Request.CommandId },
                { "st", SlarkToken.NewToken().Token},
                { "stTtl" , 2880 },
                { "appId", context.Request.Body["appId"] },
                { "peerId", context.Request.Body["peerId"] }
            };
            return Task.FromResult(context);
        }

        public Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            return context.Sender.ToEnumerableAsync();
        }

        public Task<string> SerializeAsync(ISlarkMessage message)
        {
            var request = new PlayRequest(message.MetaText);

            var response = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "opened"},
                { "i", request.CommandId },
                { "st", SlarkToken.NewToken().Token},
                { "stTtl" , 2880 },
                { "appId", request.Body["appId"] },
                { "peerId", request.Body["peerId"] }
            };

            return response.ToJsonStringAsync();
        }
    }
}
