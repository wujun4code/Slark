using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Extensions;
using Slark.Core.Protocol;
using TheMessage.Protocols;
using System.Linq;
using LeanCloud;
using LeanCloud.Storage.Internal;
using LeanCloud.Core.Internal;
using TheMessage.Extensions;

namespace TheMessage
{
    [RpcHost]
    public class TMLobby : SlarkStandardServer, IRpc
    {
        static TMLobby()
        {
            AVObject.RegisterSubclass<TMJsonRequest>();
            AVObject.RegisterSubclass<TMRoom>();
            AVObject.RegisterSubclass<TMClientInfoInRoom>();
            AVObject.RegisterSubclass<TMGame>();
            AVObject.RegisterSubclass<TMPlayer>();
            AVObject.RegisterSubclass<TMCharacter>();
        }

        public TMLobby()
        {
            ProtocolMatcher = new TMProtocolMatcher();
            RoomContainer = new TMRoomContainer()
            {
                Lobby = this
            };
        }

        public TMRoomContainer RoomContainer { get; set; }

        public IDictionary<string, ISlarkProtocol> RpcFuncs = new Dictionary<string, ISlarkProtocol>();

        public void Register(string functionName, ISlarkProtocol functionHandler)
        {
            RpcFuncs[functionName] = functionHandler;
            Console.WriteLine($"rpc.Register:{functionName}");
        }
        public void Unregister(string functionName)
        {
            RpcFuncs.Remove(functionName);
            Console.WriteLine($"rpc.Unregister:{functionName}");
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

        public override Task<SlarkClient> CreateClient(SlarkClientConnection slarkClientConnection)
        {
            return Task.FromResult(new TMClient() as SlarkClient);
        }

        public override ISlarkMessage CreateMessage(string message)
        {
            if (Json.Parse(message) is IDictionary<string, object> result)
            {
                if (result.ContainsKey("results") && result.ContainsKey("si"))
                {
                    return result["results"] is Dictionary<string, object> results
                        ? new TMJsonResponse(message, results, result["si"].ToString())
                            : new TMJsonResponse(message, result["si"].ToString());
                }
                var state = AVObjectCoder.Instance.Decode(result, AVDecoder.Instance);
                return AVObject.FromState<TMJsonRequest>(state, result["className"] as string);
            }
            return base.CreateMessage(message);
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
            try
            {
                if (context.Message is TMJsonRequest request)
                {
                    var method = request.Url.Split("/").Last();
                    if (!RpcFuncs.ContainsKey(method)) throw new MissingMethodException($"MissingMethodException OnRpcRedicrect on {method}");
                    return RpcFuncs[method].ExecuteAsync(context);
                }
                return context.ReplyAsync();
            }
            catch (Exception ex)
            {
                var errorJson = new Dictionary<string, object>();
                if (ex is MissingMethodException mmex)
                {
                    errorJson["code"] = 404;
                    errorJson["message"] = mmex.Message;
                }
                return context.ReplyAsync();
            }
        }

        [Rpc]
        public async Task<AVUser> LogIn(string username, string password, TMContext context)
        {
            Console.WriteLine($"username:{username}, password:{password}");
            var user = await AVUser.LogInAsync(username, password);
            context.Client.User = user;
            return user;
        }

        [Rpc]
        public async Task<TMRoom> CreateRoom(TMRoom room, TMContext context)
        {
            await RoomContainer.PostAsync(room);
            room.UseRpc(this);
            room.Lobby = this;
            room.JoinRoom(context);
            room.SetRoomHost(context);
            return room;
        }

        [Rpc]
        public async Task<TMRoom> QuickJoin(TMContext context)
        {
            var room = await RoomContainer.GetQuickStartRoom();
            if (room.Status == TMRoom.RoomStatus.Created)
            {
                room.UseRpc(this);
                room.JoinRoom(context);
                room.Lobby = this;
                room.SetRoomHost(context);
            }
            else
            {
                room.JoinRoom(context);
            }
            return room;
        }

        [Rpc]
        public async Task WorldTextMessage(TMClient client, string message, TMContext context)
        {
            var clients = this.Connections.Select(c => c.Client as TMClient);
            await this.RpcAsync(clients, "WorldMessage", client.User, message);
        }

        public override Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            if (slarkClientConnection.Client is TMClient client)
            {
                RoomContainer.ClientDisconnectLobby(client);
            }

            return base.OnDisconnected(slarkClientConnection);
        }

        //[Request("/room", "POST")]
        //public async Task CreateRoom(SlarkContext context)
        //{
        //    if (context.Message is TMJsonRequest request)
        //    {
        //        var roomJson = request.Body.ToJsonString();
        //        var room = JsonConvert.DeserializeObject<TMRoom>(roomJson);
        //        await RoomServer.PostAsync(room);
        //        context.Response = JsonConvert.SerializeObject(room);
        //        return;
        //    }
        //    await context.ReplyAsync();
        //}
    }
}