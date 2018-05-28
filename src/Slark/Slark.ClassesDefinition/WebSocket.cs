using System;
namespace Slark.ClassesDefinition
{
    public interface ISlarkWebSocketClient
    {
        bool IsOpen
        {
            get;
        }

        event Action<int, string, string> OnClosed;

        event Action<string> OnError;

        event Action<string> OnLog;

        event Action<string> OnMessage;

        event Action OnOpened;

        void Close();

        void Open(string url, string protocol = null);

        void Send(string message);
    }

    public interface ISlarkWebSocketServer
    {
        
    }
}
