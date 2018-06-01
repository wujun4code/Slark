using Slark.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayServer : SlarkStandardServer
    {
        public IDictionary<string, IPlayRouteHandler> RouterHandlers { get; set; }
        public IDictionary<string, IPlayCommandHandler> CommandHandlers { get; set; }

        public PlayServer()
        {
            RouterHandlers = new Dictionary<string, IPlayRouteHandler>();
            AddRouteHandler(new LobbyRouterHandler());

            CommandHandlers = new Dictionary<string, IPlayCommandHandler>();
        }

        public void AddRouteHandler(IPlayRouteHandler playRouteHandler)
        {
            RouterHandlers.Add(playRouteHandler.Router, playRouteHandler);
        }

        public void AddCommandHandler(IPlayCommandHandler playCommandHandler)
        {
            CommandHandlers.Add(playCommandHandler.Command + "-" + playCommandHandler.Operation, playCommandHandler);
        }

        public override async Task OnReceived(SlarkClientConnection slarkClientConnection, string message)
        {
            var request = new PlayRequest(message);
            var context = new PlayContext()
            {
                Request = request,
                Response = new PlayResponse()
            };
            if (CommandHandlers.ContainsKey(request.CommandHandlerKey))
            {
                await CommandHandlers[request.CommandHandlerKey].ExecuteAsync(context);
                await slarkClientConnection.SendAsync(context.Response.Body.ToJsonString());
            }
        }

        public override Task<string> OnRequest(string route, string message)
        {
            if (RouterHandlers.ContainsKey(route))
            {
                return RouterHandlers[route].Response(message);
            }
            return Task.FromResult(message);
        }
    }
}
