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

        public override IEnumerable<SlarkClientConnection> Connections => this.DecoratedServer.Connections;

        public override string ServerUrl { get => this.DecoratedServer.ServerUrl; set => this.DecoratedServer.ServerUrl = value; }

        public SlarkWebSokcetServer(SlarkServer slarkServer, string hostingUrl = null, string routePath = null)
            : base(slarkServer)
        {
            if (!string.IsNullOrEmpty(routePath)) this.RoutePath = routePath;
            if (string.IsNullOrEmpty(hostingUrl))
            {
                hostingUrl = "localhost:5000";
            }
            slarkServer.ServerUrl = hostingUrl + this.RoutePath;
        }

        public SlarkWebSokcetServer(SlarkServer slarkServer) : this(slarkServer, null, null)
        {

        }

        public SlarkWebSocketClientConnection FromWebSocket(HttpContext context, WebSocket webSocket)
        {
            return new SlarkWebSocketClientConnection(webSocket);
        }

        public async Task<SlarkWebSocketClientConnection> OnWebSocketConnected(HttpContext context, WebSocket webSocket)
        {
            var clientConnection = FromWebSocket(context, webSocket);
            this.AddConnectionSync(clientConnection);
            await this.OnConnected(clientConnection);
            return clientConnection;
        }

        public async Task OnDisconnected(SlarkWebSocketClientConnection connection)
        {
            this.RemoveConnectionSync(connection);
            await OnDisconnected(connection as SlarkClientConnection);
        }

        public async Task OnWebSocketInvoked(SlarkWebSocketClientConnection connection)
        {
            try
            {
                while (connection.WebSocket.State == WebSocketState.Open)
                {
                    ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
                    WebSocketReceiveResult result = null;
                    try
                    {
                        if (!connection.WebSocket.CloseStatus.HasValue)
                        {
                            using (var ms = new MemoryStream())
                            {
                                do
                                {
                                    result = await connection.WebSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                                }
                                while (!result.EndOfMessage);

                                ms.Seek(0, SeekOrigin.Begin);

                                using (var reader = new StreamReader(ms, Encoding.UTF8))
                                {
                                    string message = await reader.ReadToEndAsync().ConfigureAwait(false);
                                    await OnReceived(connection, message);
                                }
                            }
                        }
                    }

                    catch (WebSocketException websocketEx)
                    {
                        if (websocketEx.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                        {
                            connection.WebSocket.Abort();
                        }
                    }
                }
                await this.OnDisconnected(connection);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




            //var buffer = new byte[BufferSize];

            //while (connection.WebSocket.State == WebSocketState.Open)
            //{
            //    var result = await connection.WebSocket.ReceiveAsync(
            //        buffer: new ArraySegment<byte>(buffer),
            //        cancellationToken: CancellationToken.None);

            //    if (result.MessageType == WebSocketMessageType.Text)
            //    {
            //        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            //        await OnReceived(connection, message);
            //    }
            //    else if (result.MessageType == WebSocketMessageType.Close)
            //    {
            //        await this.OnDisconnected(connection);
            //    }
            //}
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

        public override void AddConnectionSync(SlarkClientConnection connection)
        {
            this.DecoratedServer.AddConnectionSync(connection);
        }

        public override void RemoveConnectionSync(SlarkClientConnection connection)
        {
            this.DecoratedServer.RemoveConnectionSync(connection);
        }
    }
}
