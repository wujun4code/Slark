using Microsoft.AspNetCore.Builder;
using Slark.Server.LeanCloud.Play;
using Slark.Server.WebSoket;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Slark.Server.ConsoleApp.NETCore
{
    public static class SlarkWebSocketServerStartup
    {
        public static IApplicationBuilder UseSlarkWebSokcetServer(this IApplicationBuilder app, SlarkWebSokcetServer slarkWebSokcetServer)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path != slarkWebSokcetServer.WebSocketRoutePath)
                {
                    await next.Invoke();
                    return;
                }

                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                var connection = await slarkWebSokcetServer.OnWebSocketConnected(context, webSocket);
                if (connection != null)
                {
                    await slarkWebSokcetServer.OnWebSocketInvoked(connection);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            });
            return app;
        }
    }
}
