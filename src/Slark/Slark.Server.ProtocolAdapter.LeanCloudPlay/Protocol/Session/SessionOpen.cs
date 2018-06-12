using Slark.Core;
using Slark.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class SessionOpen : PlayCommandHandlerBase
    {
        public override string Command { get; set; } = "session";
        public override string Operation { get; set; } = "open";

        public override async Task<string> ResponseAsync(PlayRequest request, SlarkContext context)
        {
            context.Sender.Client.Token = SlarkToken.NewToken();
            if (context.Server is PlayLobbyServer lobby)
            {
                if (context.Sender.Client is PlayClient client)
                {
                    client.PeerId = request.Body["peerId"].ToString();
                    lobby.TokenClientMap.TryAdd(context.Sender.Client.Token.Token, client);
                }
            }
            else if (context.Server is PlayGameServer gameServer)
            {
                var token = request.Body["st"].ToString();
                if (context.Sender.Client is PlayClient client)
                {
                    context.Sender.Client = await gameServer.LobbyServer.FindClientAsync(token);
                }
            }

            var response = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "opened"},
                { "i", request.CommandId },
                { "st", context.Sender.Client.Token.Token},
                { "stTtl" , 2880 },
                { "appId", request.Body["appId"] },
                { "peerId", request.Body["peerId"] }
            };

            return response.ToJsonString();
        }
    }
}
