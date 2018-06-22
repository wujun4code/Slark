using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Slark.Server.WebSoket;
using System;

namespace Slark.Server.ConsoleApp.NETCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) => {
            //    context.Request.EnableRewind();
            //    await next();
            //});

            var playGameServer = new LeanCloud.Play.PlayGameServer();

            playGameServer.UseWebSocket("/game");

            var gameServers = new LeanCloud.Play.PlayGameServer[] { playGameServer };

            var playLobbyServer = new LeanCloud.Play.PlayLobbyServer(gameServers);

            playLobbyServer.UseWebSocket("/lobby");

            //var liveBoardcastServer = new Slark.Server.WebSoket.SlarkWebSokcetServer(new Slark.Server.LiveBroadcast.LiveBroadcastServer(), hostingUrl, "/nba");
            //app.UseSlarkWebSokcetServer(liveBoardcastServer);

            var routeBuilder = new RouteBuilder(app);

            routeBuilder.MapGet("play/v1/{router}", async context =>
            {
                var router = context.GetRouteValue("router").ToString();
                var message = context.Request.QueryString.Value;
                var rpcParameters = message.Split('?');
                var response = await playLobbyServer.OnRPC(router, message);
                await context.Response.WriteAsync(response);
            });

            routeBuilder.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("hello!");
            });

            var routes = routeBuilder.Build();
            app.UseRouter(routes);


        }
    }
}
