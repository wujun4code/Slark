using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Slark.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slark.Server.WebSoket
{
    public class SlarkWebSokcetServer : SlarkServerDecorator
    {
        public string RoutePath { get; set; } = "/ws";
        protected int BufferSize { get => 1024 * 4; }


        public SlarkWebSokcetServer(SlarkServer slarkServer) : base(slarkServer)
        {

        }

        public SlarkWebSokcetServer(SlarkServer slarkServer, string routePath) : this(slarkServer)
        {
            this.RoutePath = routePath;
        }

        public override List<SlarkClientConnection> Connections
        {
            get => DecoratedServer.Connections; set => DecoratedServer.Connections = value;
        }

        public SlarkWebSocketClientConnection FromWebSocket(HttpContext context, WebSocket webSocket)
        {
            return new SlarkWebSocketClientConnection(webSocket);
        }

        public async Task<SlarkWebSocketClientConnection> OnWebSocketConnected(HttpContext context, WebSocket webSocket)
        {
            var clientConnection = FromWebSocket(context, webSocket);
            this.Connections.Add(clientConnection);
            await this.OnConnected(clientConnection);
            return clientConnection;
        }

        public async Task OnDisconnected(SlarkWebSocketClientConnection connection)
        {
            await OnDisconnected(connection as SlarkClientConnection);
        }

        public async Task OnWebSocketInvoked(SlarkWebSocketClientConnection connection)
        {
            var buffer = new byte[BufferSize];

            while (connection.WebSocket.State == WebSocketState.Open)
            {
                var result = await connection.WebSocket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await OnReceived(connection, message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await this.OnDisconnected(connection);
                }
            }
        }

        public override async Task OnConnected(SlarkClientConnection slarkClientConnection)
        {
            await this.DecoratedServer.OnConnected(slarkClientConnection);
        }

        public override async Task OnReceived(SlarkClientConnection slarkClientConnection, string message)
        {
            await this.DecoratedServer.OnReceived(slarkClientConnection, message);
        }

        public override async Task OnDisconnected(SlarkClientConnection slarkClientConnection)
        {
            await this.DecoratedServer.OnDisconnected(slarkClientConnection);
        }

        public override Task<string> OnRPC(string method, params object[] rpcParamters)
        {
            return this.DecoratedServer.OnRPC(method, rpcParamters);
        }
    }
}
