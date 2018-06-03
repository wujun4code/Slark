using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play.Protocol.Room
{
    public class RoomAllocate : IPlayCommandHandler
    {
        public string Command { get; set; } = "conv";
        public string Operation { get; set; } = "start";

        public Task ExecuteAsync(PlayContext context)
        {
            context.Response.Body = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "started"},
                { "i", context.Request.CommandId },
                { "addr", "ws://localhost:5000/ws" },
                { "secureAddr" , "ws://localhost:5000/ws" },
            };
            return Task.FromResult(context);
        }
    }
}
