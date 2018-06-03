using Slark.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play.Protocol.Session
{
    public class SessionOpen : IPlayCommandHandler
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
    }
}
