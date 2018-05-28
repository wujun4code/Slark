using System;
using System.Collections.Generic;

namespace Slark.ClassesDefinition
{
    public class SlarkRole : IAttachable
    {
        public SlarkRole()
        {
            OnAttached += SlarkRole_OnAttached;
            UndistributedConnections = new List<SlarkConnection<SlarkNode>>();
        }

        private ISlarkWebSocketClient webSocketClientAdapter;
        public ISlarkWebSocketClient WebSocketClientAdapter
        {
            get
            {
                return webSocketClientAdapter;
            }
            set
            {
                webSocketClientAdapter = value;
                if (value != null)
                {
                    webSocketClientAdapter.OnMessage += _webSocket_OnMessage;
                    webSocketClientAdapter.OnOpened += _webSocketAdapter_OnOpened;
                }
            }
        }

        private string connectingAddress;
        public void Connect(string address)
        {
            connectingAddress = address;
            WebSocketClientAdapter.Open(address);
        }

        public event Action<SlarkNode> OnConnected;

        void _webSocketAdapter_OnOpened()
        {
            if (OnConnected != null)
            {
                SlarkNode node = new SlarkNode()
                {
                    Address = connectingAddress
                };
                OnConnected.Invoke(node);
            }
        }

        public event Action<SlarkCommand> OnCommand;

        void _webSocket_OnMessage(string obj)
        {
            if (OnCommand != null)
            {
                var command = SlarkCommand.Received(obj);
                OnCommand.Invoke(command);
            }
        }

        public void Send(SlarkCommand command)
        {
            var message = command.Body.ToJsonString();
            WebSocketClientAdapter.Send(message);
        }

        public event Action<SlarkNode> OnAttached;
        public List<SlarkConnection<SlarkNode>> UndistributedConnections { get; set; }
        void SlarkRole_OnAttached(SlarkNode obj)
        {
            UndistributedConnections.Add(new SlarkConnection<SlarkNode>()
            {
                TheOtherSide = obj
            });
        }

    }
}
