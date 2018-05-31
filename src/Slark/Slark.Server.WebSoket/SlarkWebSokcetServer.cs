using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Slark.Core;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slark.Server.WebSoket
{
    public class SlarkWebSokcetServer : SlarkServer
    {
        public string RoutePath { get; set; } = "/ws";
        protected int BufferSize { get => 1024 * 4; }

        public SlarkWebSocketClientConnection FromWebSocket(HttpContext context, WebSocket webSocket)
        {
            return new SlarkWebSocketClientConnection(webSocket);
        }

        public async Task<SlarkWebSocketClientConnection> OnWebSocketConnected(HttpContext context, WebSocket webSocket)
        {
            var clientConnection = FromWebSocket(context, webSocket);
            await ProtocolAdapter.OnConnected(clientConnection);
            this.Connections.Add(clientConnection);
            return clientConnection;
        }

        public async Task OnDisconnected(SlarkWebSocketClientConnection connection)
        {
            await ProtocolAdapter.OnDisconnected(connection);
            this.Connections.Remove(connection);
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
                    await ProtocolAdapter.OnReceived(connection, message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await this.OnDisconnected(connection);
                }
            }
        }
    }
}
