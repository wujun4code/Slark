using System;
using LeanCloud.Engine;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Slark.Server.LeanCloud.Play;
using Slark.Server.WebSoket;
using TheMessage;
using Slark.Core.Extensions;

namespace Slark.Server.ConsoleApp.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseCloud(ConfigCloud, ConfigApp)
                   .Build();

        public static void ConfigCloud(Cloud cloud)
        {
            cloud.UseLog();
            cloud.UseHttpsRedirect();
        }

        public static void ConfigApp(IApplicationBuilder app)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            app.UseLog();

            var hostingUrlWithSchema = Cloud.Singleton.IsProduction ? Cloud.Singleton.GetHostingUrl() : "http://localhost:3000";

            var playGameServer = new TMGameServer();

            var playGameWebSocketServer = playGameServer.UseWebSocket(hostingUrlWithSchema, "/game");

            app.UseSlarkWebSokcetServer(playGameWebSocketServer);

            var gameServers = new PlayGameServer[] { playGameServer };

            var playLobbyServer = new TMLobby(gameServers).Inject<TMLobby>();

            var playLobbyWebSocketServer = playLobbyServer.UseWebSocket(hostingUrlWithSchema, "/lobby");
            playLobbyWebSocketServer.ToggleLog = true;
            app.UseSlarkWebSokcetServer(playLobbyWebSocketServer);

            var routeBuilder = new RouteBuilder(app);

            routeBuilder.MapGet("play/v1/{router}", async context =>
            {
                var router = context.GetRouteValue("router").ToString();
                var message = context.Request.QueryString.Value;
                var rpcParameters = message.Split('?');
                var response = await playLobbyServer.OnRPC(router, message);
                await context.Response.WriteAsync(response);
            });

            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }
    }
}
