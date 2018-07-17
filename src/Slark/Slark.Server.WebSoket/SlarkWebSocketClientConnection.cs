using Slark.Core;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slark.Server.WebSoket
{
    public class SlarkWebSocketClientConnection : SlarkClientConnection
    {
        public WebSocket WebSocket { get; set; }

        public SlarkWebSokcetServer Server { get; set; }

        public SlarkWebSocketClientConnection(WebSocket webSocket)
        {
            WebSocket = webSocket;
            Id = Guid.NewGuid().ToString();
        }

        public override async Task SendAsync(string message)
        {
            if (Server.ToggleLog) Console.WriteLine(message);
            if (WebSocket.State != WebSocketState.Open) return;
            var arr = Encoding.UTF8.GetBytes(message);

            var buffer = new ArraySegment<byte>(
                    array: arr,
                    offset: 0,
                    count: arr.Length);

            await WebSocket.SendAsync(
                buffer: buffer,
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }
    }
}
