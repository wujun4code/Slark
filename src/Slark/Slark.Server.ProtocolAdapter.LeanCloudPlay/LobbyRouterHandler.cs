using Slark.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public class LobbyRouterHandler : IPlayRouteHandler
    {
        public string Router { get; set; } = "router";

        public Task<string> Response(string message)
        {
            var lobbyRouterResponse = new Dictionary<string, object>()
            {
                { "server", "ws://localhost:5000/ws"},
                { "secondary", "ws://localhost:5000/ws"},
                { "ttl", 1440}
            };
            return Task.FromResult(lobbyRouterResponse.ToJsonString());
        }
    }
}
