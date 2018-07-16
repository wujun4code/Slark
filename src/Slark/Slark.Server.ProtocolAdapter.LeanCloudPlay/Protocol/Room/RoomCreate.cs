using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomCreate : PlayCommandHandlerBase
    {
        public override string Command { get; set; } = "conv";
        public override string Operation { get; set; } = "start";

        public override async Task<string> ResponseAsync(SlarkContext context)
        {
            var request = context.Message as PlayRequest;

            var responseBody = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "started" },
                { "i", request.CommandId },
            };

            var roomConfig = JsonConvert.DeserializeObject<RoomConfig>(context.Message.MetaText);

            if (context.Server is PlayGameServer gameServer)
            {
                if (context.Sender.Client is PlayClient client)
                {
                    var room = await gameServer.CreateWithConfigAsync(roomConfig, context.Sender);

                    responseBody.Add("cid", room.Id);
                    responseBody.Add("visible", room.IsVisible);
                    responseBody.Add("masterClientId", room.MasterClientId);
                    responseBody.Add("memberIds", room.ActorIds);
                    responseBody.Add("actorIds", room.ActorIds);
                    responseBody.Add("m", room.MemberIds);
                    responseBody.Add("masterActorId", room.MasterClient.ActorId);
                    responseBody.Add("ttlSecs", room.TimeToKeep);
                    responseBody.Add("open", room.IsOpen);
                    responseBody.Add("emptyRoomTtl", room.EmptyTimeToLive);
                    responseBody.Add("expectMembers", room.ExpectedMemberPeerIds);
                    responseBody.Add("maxMembers", room.MaxPlayerCount);
                    responseBody.Add("members", room.MembersJsonFormatting);
                }
            }

            return responseBody.ToJsonString();
        }
    }
}
