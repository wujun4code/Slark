using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Newtonsoft.Json;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomJoin : PlayCommandHandlerBase
    {
        public override string Command { get; set; } = "conv";
        public override string Operation { get; set; } = "add";

        public async override Task<string> ResponseAsync( SlarkContext context)
        {
            var request = context.Message as PlayRequest;

            if (context.Server is PlayGameServer gameServer)
            {
                request.TryGet<string>("cid", "", out string cid);
                gameServer.Rooms.TryGet(r => r.Id == cid, out PlayRoom room);

                if (room != null)
                {
                    context.HasNotice = true;
                    var tuple = await room.NewPlayerJoinAsync(request, context.Sender);

                    context.Response = tuple.Item1.MetaText;
                    context.Notice = tuple.Item2.MetaText;

                    context.Receivers = room.AvailableConnections;
                }
            }
            else if (context.Server is PlayLobbyServer lobby)
            {
                var responseBody = new Dictionary<string, object>()
                {
                    { "cmd", this.Command },
                    { "op", "added" },
                    { "i", request.CommandId },
                };

                var joinRequest = JsonConvert.DeserializeObject<RoomJoinRequest>(request.MetaText);

                var tuple = await lobby.MatchAsync(joinRequest);

                if (tuple.Item1 != null)
                {
                    var gs = tuple.Item1;
                    responseBody.Add("addr", gs.ClientConnectingAddress);
                    responseBody.Add("secureAddr", gs.ClientConnectingAddress);
                }

                if (tuple.Item2 != null)
                {
                    var room = tuple.Item2;
                    responseBody.Add("cid", room.Id);
                    responseBody.Add("open", room.IsOpen);
                    responseBody.Add("m", room.MemberIds);
                    responseBody.Add("memberIds", room.ActorIds);
                    responseBody.Add("visible", room.IsVisible);
                    responseBody.Add("maxMembers", room.MaxPlayerCount);
                }

                var response = new PlayResponse(responseBody);
                response.Timestamplize();
                response.SerializeBody();
                return response.MetaText;
            }

            return await base.ResponseAsync(context);
        }
    }
}
