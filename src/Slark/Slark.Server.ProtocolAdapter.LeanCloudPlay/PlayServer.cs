using Slark.Core;
using Slark.Server.LeanCloud.Play.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayServer : SlarkStandardServer
    {
        public IDictionary<string, IPlayRPCHandler> RPCHandlers { get; set; }
        public IDictionary<string, IPlayCommandHandler> CommandHandlers { get; set; }

        public PlayServer()
        {
            RPCHandlers = new Dictionary<string, IPlayRPCHandler>();
            AddRPCHandler(new LobbyRouterHandler());

            var protocolMatcher = new PlayProtocolMatcher();
            protocolMatcher.AddCommandHandler(new SessionOpen());
            protocolMatcher.AddCommandHandler(new RoomAllocate());

            ProtocolMatcher = protocolMatcher;
        }

        public void AddRPCHandler(IPlayRPCHandler playRouteHandler)
        {
            RPCHandlers.Add(playRouteHandler.Method, playRouteHandler);
        }

        public override async Task OnReceived(SlarkClientConnection slarkClientConnection, string message)
        {
            //var request = new PlayRequest(message);

            //foreach (var validator in Validators)
            //{
            //    var valid = await validator.Validate(request);
            //    if (!valid)
            //    {
            //        await validator.OnInvalid(slarkClientConnection, request);
            //        return;
            //    }
            //}

            //var context = new PlayContext()
            //{
            //    Server = this,
            //    Request = request,
            //    Response = new PlayResponse()
            //};

            //if (CommandHandlers.ContainsKey(request.CommandHandlerKey))
            //{
            //    await CommandHandlers[request.CommandHandlerKey].ExecuteAsync(context);
            //    await slarkClientConnection.SendAsync(context.Response.Body.ToJsonString());
            //}
            await base.OnReceived(slarkClientConnection, message);
        }

        public override Task<string> OnRPC(string method, params object[] rpcParamters)
        {
            if (RPCHandlers.ContainsKey(method))
            {
                return RPCHandlers[method].RPC(rpcParamters);
            }
            return this.OnRPC(method, rpcParamters);
        }
    }
}
