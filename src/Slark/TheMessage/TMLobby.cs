using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Extensions;
using Slark.Core.Protocol;
using TheMessage.Protocols;
using Newtonsoft.Json;
using System.Linq;

namespace TheMessage
{
    public class TMLobby : SlarkStandardServer
    {
        public TMLobby()
        {
            ProtocolMatcher = new TMProtocolMatcher();
        }

        public TMRoomServer RoomServer { get; set; } = new TMRoomServer();

        public IDictionary<string, ISlarkProtocol> RpcFuncs = new Dictionary<string, ISlarkProtocol>();

        public void Register(string functionName, ISlarkProtocol functionHandler)
        {
            RpcFuncs[functionName] = functionHandler;
        }

        public virtual Task InvokeRpc(string functionName, SlarkContext context)
        {
            if (RpcFuncs.ContainsKey(functionName))
            {
                var function = RpcFuncs[functionName];
                return function.ExecuteAsync(context);
            }
            return Task.FromResult("");
        }

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

        [Request("/rpc/")]
        public Task OnRpcRedicrect(SlarkContext context)
        {
            if (context.Message is TMJsonRequest request)
            {
                var method = request.Url.Split("/").Last();
                if (!RpcFuncs.ContainsKey(method)) throw new MissingMethodException($"MissingMethodException OnRpcRedicrect on {method}");
                return RpcFuncs[method].ExecuteAsync(context);
            }
            return context.ReplyAsync();
        }

        [Rpc("login")]
        public Task LogIn(string username, string password)
        {
            Console.WriteLine($"username:{username},password:{password}");
            return Task.FromResult(0);
        }

        [Rpc("createRoom")]
        public async Task CreateRoom(TMRoom room)
        {
            Console.WriteLine($"room.GameMode:{room.GameMode}");
            await RoomServer.PostAsync(room);
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