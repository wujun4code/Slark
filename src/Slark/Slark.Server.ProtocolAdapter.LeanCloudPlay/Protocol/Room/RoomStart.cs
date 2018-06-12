using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomStart : PlayCommandHandlerBase
    {
        public override string Command { get; set; } = "conv";
        public override string Operation { get; set; } = "start";

        public override Task<string> ResponseAsync(PlayRequest request, SlarkContext context)
        {
            var response = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "started" },
                { "i", request.CommandId },
            };

            if (context.Server is PlayLobbyServer lobby)
            {
                var randomOne = lobby.GameServerUrls.RandomOne();
                response.Add("addr", "ws://" + randomOne);
                response.Add("secureAddr", "ws://" + randomOne);
            }

            return response.ToJsonStringAsync();
        }
    }
}
