using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Extensions;
using Slark.Core.Protocol;
using TheMessage.Protocols;
using Newtonsoft.Json;

namespace TheMessage
{
    public class TMLobby : SlarkStandardServer
    {
        public TMLobby()
        {
            ProtocolMatcher = new TMProtocolMatcher();
        }

        public TMRoomServer RoomServer { get; set; } = new TMRoomServer();

        [Injective("CreateClientAsync")]
        public Task<SlarkClient> Create(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(new TMClient() as SlarkClient);
        }

        public override ISlarkMessage CreateMessage(string message)
        {
            var request = JsonConvert.DeserializeObject<TMJsonRequest>(message);
            request.MetaText = message;
            return request;
        }

        public override SlarkContext CreateContext(SlarkClientConnection slarkClientConnection, string message)
        {
            var context = new TMContext()
            {
                Server = this,
                Message = CreateMessage(message),
                Sender = slarkClientConnection,
            };
            return context;
        }

        [Request("/login")]
        public async Task LogIn(SlarkContext context)
        {
            if (context.Message is TMJsonRequest request)
            {
                var data = new Dictionary<string, object>()
                {
                    { "echo", $"welcome, {request.Body["username"]}"},
                    { "i", request.CommandId },
                    { "auth",true },
                    { "connId",context.Sender.Id }
                };
                context.Response = SlarkCorePlugins.Singleton.Encoder.Encode(data);
                return;
            }
            await context.ReplyAsync();
        }

        [Request("/room", "POST")]
        public async Task CreateRoom(SlarkContext context)
        {
            if (context.Message is TMJsonRequest request)
            {
                var roomJson = request.Body.ToTMJsonString();
                var room = JsonConvert.DeserializeObject<TMRoom>(roomJson);
                await RoomServer.PostAsync(room);
                context.Response = JsonConvert.SerializeObject(room);
                return;
            }
            await context.ReplyAsync();
        }
    }
}