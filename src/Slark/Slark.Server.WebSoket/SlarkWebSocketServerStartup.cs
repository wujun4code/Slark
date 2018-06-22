﻿using Microsoft.AspNetCore.Builder;
using Slark.Core;
using System.Net.WebSockets;

namespace Slark.Server.WebSoket
{
    public static class SlarkWebSocketServerStartup
    {
        public static SlarkWebSokcetServer UseWebSocket(this SlarkServer slarkServer, string serviceRelativeRoute)
        {
            var hostingUrl = "localhost:5000";
            var webSocketServer = new Slark.Server.WebSoket.SlarkWebSokcetServer(slarkServer, hostingUrl, serviceRelativeRoute);
            return webSocketServer;
        }

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
