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

        public override async Task<string> ResponseAsync(PlayRequest request, SlarkContext context)
        {
            var response = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "started" },
                { "i", request.CommandId },
            };

            var roomConfig = JsonConvert.DeserializeObject<RoomConfig>(context.Message.MetaText);

            if (context.Server is PlayGameServer game)
            {
                if (context.Sender.Client is PlayClient client)
                {
                    var room = await game.CreateWithConfigAsync(roomConfig, client);

                    response.Add("cid", room.Id);
                    response.Add("visible", room.IsVisible);
                    response.Add("masterClientId", room.MasterClientId);
                    response.Add("memberIds", room.ActorIds);
                    response.Add("actorIds", room.ActorIds);
                    response.Add("m", room.MemberIds);
                    response.Add("masterActorId", room.MasterClient.ActorId);
                    response.Add("ttlSecs", room.TimeToKeep);
                    response.Add("open", room.IsOpen);
                    response.Add("emptyRoomTtl", room.EmptyTimeToLive);
                    response.Add("expectMembers", room.ExpectedMemberPeerIds);
                    response.Add("maxMembers", room.MaxPlayerCount);
                    response.Add("members", room.MembersJsonFormatting);
                }
            }
            return response.ToJsonString();
        }
    }
}
