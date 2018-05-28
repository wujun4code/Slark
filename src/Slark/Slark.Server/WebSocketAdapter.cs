using System;
using Slark.ClassesDefinition;

namespace Slark.Server
{
    public class WebSocketAdapter : ISlarkWebSocketClient
    {
        public WebSocketAdapter()
        {

        }

        public bool IsOpen { get; }

        public event Action<int, string, string> OnClosed;
        public event Action<string> OnError;
        public event Action<string> OnLog;
        public event Action<string> OnMessage;
        public event Action OnOpened;

        public void Close()
        {
            
        }

        public void Open(string url, string protocol = null)
        {
            
        }

        public void Send(string message)
        {
            
        }
    }
}
