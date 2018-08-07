using Microsoft.AspNetCore.Http;
using Slark.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slark.Server.WebSoket
{
    public class SlarkWebSokcetServer : SlarkServerDecorator
    {
        public string WebSocketRoutePath { get; set; } = "/ws";

        public bool ToggleLog { get; set; }

        protected int BufferSize { get => 1024 * 4; }

        public override IEnumerable<SlarkClientConnection> Connections => this.DecoratedServer.Connections;

        public override string ServerUrl { get => this.DecoratedServer.ServerUrl; set => this.DecoratedServer.ServerUrl = value; }
        public override string ClientConnectionUrl { get => this.DecoratedServer.ClientConnectionUrl; set => this.DecoratedServer.ClientConnectionUrl = value; }

        public SlarkWebSokcetServer(SlarkServer slarkServer, string httpSchema, string hostWithPort, string websocketRoutePath = null)
            : base(slarkServer)
        {
            if (!string.IsNullOrEmpty(websocketRoutePath)) this.WebSocketRoutePath = websocketRoutePath;
            slarkServer.ServerUrl = $"{httpSchema}://{hostWithPort}";
            var wsSchema = httpSchema.ToLower().Equals("http") ? "ws" : "wss";
            slarkServer.ClientConnectionUrl = $"{wsSchema}://{hostWithPort}{WebSocketRoutePath}";
        }

        public SlarkWebSokcetServer(SlarkServer slarkServer) : this(slarkServer, null, null)
        {

        }

        public SlarkWebSocketClientConnection FromWebSocket(HttpContext context, WebSocket webSocket)
        {
            return new SlarkWebSocketClientConnection(this, webSocket);
        }

        public async Task<SlarkWebSocketClientConnection> OnWebSocketConnected(HttpContext context, WebSocket webSocket)
        {
            var clientConnection = FromWebSocket(context, webSocket);
            this.AddConnectionSync(clientConnection);
            await this.OnConnected(clientConnection);
            clientConnection.Client.Courier = clientConnection.SendAsync;
            return clientConnection;
        }

        public async Task OnDisconnected(SlarkWebSocketClientConnection connection)
        {
            this.RemoveConnectionSync(connection);
            await OnDisconnected(connection as SlarkClientConnection);
        }

        public async Task OnWebSocketInvoked(HttpContext context, SlarkWebSocketClientConnection connection)
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
                                    if (ToggleLog)
                                    {
                                        var request = context.Request;
                                        string log = $"{request.Method} {request.Scheme}://{request.Host}{request.Path} {message}";
                                        Log(log);
                                    }

                                    await OnReceived(connection, message);
                                }
                            }
                        }
                        else
                        {
                            if (connection.WebSocket.State == WebSocketState.Aborted)
                            {
                                // Handle aborted
                                await OnDisconnected(connection);
                            }
                            else if (connection.WebSocket.State == WebSocketState.Closed)
                            {
                                await OnDisconnected(connection);
                            }
                            else if (connection.WebSocket.State == WebSocketState.CloseReceived)
                            {

                            }
                            else if (connection.WebSocket.State == WebSocketState.CloseSent)
                            {
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
                if (connection.WebSocket.CloseStatus.HasValue)
                {
                    await this.OnDisconnected(connection);
                }
                if (ex.Source == "Microsoft.AspNetCore.WebSockets.Protocol" && ex.Message == "Unexpected end of stream")
                {
                    await this.OnDisconnected(connection);
                }

                Console.WriteLine(ex.StackTrace);
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

        public override async Task<SlarkClientConnection> OnConnected(SlarkClientConnection slarkClientConnection)
        {
            return await this.DecoratedServer.OnConnected(slarkClientConnection);
        }

        public override async Task OnReceived(SlarkClientConnection sender, string message)
        {
            await this.DecoratedServer.OnReceived(sender, message);
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
            if (ToggleLog) Log($"connection disconnected with id:{connection.Id}");
            this.DecoratedServer.RemoveConnectionSync(connection);
        }

        public void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")}: {message}");
        }
    }
}
