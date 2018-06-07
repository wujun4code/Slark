using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Slark.Core;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

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
            var SlarkWebSokcetServer = new Slark.Server.WebSoket.SlarkWebSokcetServer(new LeanCloud.Play.PlayServer());
            app.UseSlarkWebSokcetServer(SlarkWebSokcetServer);


            var liveBoardcastServer = new Slark.Server.WebSoket.SlarkWebSokcetServer(new Slark.Server.LiveBroadcast.LiveBroadcastServer(), "/nba");
            app.UseSlarkWebSokcetServer(liveBoardcastServer);

            var routeBuilder = new RouteBuilder(app);

            routeBuilder.MapGet("play/v1/{router}", async context =>
             {
                 var router = context.GetRouteValue("router").ToString();
                 var message = context.Request.QueryString.Value;
                 var rpcParameters = message.Split('?');
                 var response = await SlarkWebSokcetServer.OnRPC(router, message);
                 await context.Response.WriteAsync(response);
             });

            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
