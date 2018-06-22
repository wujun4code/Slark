using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using System.Linq;
using System.Linq.Expressions;
using static Slark.Core.SlarkExtensions;
using Newtonsoft.Json;
using static Slark.Server.LeanCloud.Play.PlayExtensions;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomUpdate : PlayCommandHandlerBase
    {
        public override string Command { get; set; } = "conv";
        public override string Operation { get; set; } = "update";

        public override Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            var request = context.Message as PlayRequest;
            if (context.Server is PlayGameServer gameServer)
            {
                request.TryGet<string>("cid", "", out string cid);
                gameServer.Rooms.TryGet(r => r.Id == cid, out PlayRoom room);
                var notice = new PlayNotice()
                {
                    Body = new Dictionary<string, object>()
                    {
                        { "cmd", this.Command },
                        { "op", "updated" },
                     }
                };

                if (context.Sender.Client is PlayClient client)
                {
                    var player = room.Players.FirstOrDefault(p => p.Client == client);
                    notice.Body.Add("initby", player.PeerId);
                    notice.Body.Add("initByActor", player.ActorId);
                    notice.Body.Add("fromSA", false);
                }
                var noticeAttr = new Hashtable();
                if (request.Body.ContainsKey("casAttr"))
                {
                    var casAttr = (JObject)request.JsonObject["casAttr"];
                    Dictionary<string, PlayCAS> cas = new Dictionary<string, PlayCAS>();
                    foreach (var p in casAttr.Properties())
                    {
                        cas.Add(p.Name, new PlayCAS()
                        {
                            ExpectedValue = p.Value["expect"],
                            ValueToSet = p.Value["value"],
                        });
                    }

                    var casNoticeAttr = room.CustomRoomProperties.AutomicUpdateOrAdd(cas);
                    noticeAttr.AutomicSet(casNoticeAttr);
                }

                if (request.Body.ContainsKey("attr"))
                {
                    var attr = (JObject)request.JsonObject["attr"];
                    var attrTable = new Hashtable();
                    foreach (var p in attr.Properties())
                    {
                        attrTable.Add(p.Name, p.Value);
                    }
                    var setNoticeAttr = room.CustomRoomProperties.AutomicSet(attrTable);
                    noticeAttr.AutomicSet(setNoticeAttr);
                }

                notice.Body["attr"] = noticeAttr;
                context.Notice = notice.Body.ToJsonString();

                return Task.FromResult(room.Players.Select(p => p.ClientConnection));
            }

            return base.GetTargetsAsync(context);
        }

        public override Task<string> ResponseAsync(SlarkContext context)
        {
            var request = context.Message as PlayRequest;
            var responseBody = new Dictionary<string, object>()
            {
                { "cmd", this.Command },
                { "op", "updated" },
                { "i", request.CommandId },
            };

            if (request.Body.ContainsKey("casAttr") || request.Body.ContainsKey("attr"))
            {
                context.HasNotice = true;
            }

            var response = new PlayResponse(responseBody);
            response.Timestamplize();
            response.SerializeBody();

            return Task.FromResult(response.MetaText);
        }
    }
}
