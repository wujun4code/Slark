using System;
using LeanCloud.Engine;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Slark.Server.WebSoket;
using TheMessage;
using Slark.Core.Extensions;
using TheMessage.Extensions;
using LeanCloud;

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
                   .UseCloud(ConfigCloud, ConfigAppCloud)
                   .Build();

        public static void ConfigCloud(Cloud cloud)
        {
            AVClient.Initialize("JRsk29cDwQNGHaIM2PM0VBWt-9Nh9j0Va", "uerPuKWcaSqHGBYVbPBYcv6V");
            cloud.UseLog();
            cloud.UseHttpsRedirect();
        }

        public static void ConfigAppCloud(IApplicationBuilder app, Cloud cloud)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            app.UseLog();

            var hostingUrlWithSchema = cloud.IsProduction ? cloud.GetHostingUrl() : "http://localhost:3000";

            TM.Init();

            var playLobbyServer = new TMLobby().UseRpc().Inject<TMLobby>();

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
