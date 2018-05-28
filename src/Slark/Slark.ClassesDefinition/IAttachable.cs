using System;
namespace Slark.ClassesDefinition
{
    public interface IAttachable
    {
        void Connect(string address);
        event Action<SlarkNode> OnConnected;

        void Send(SlarkCommand command);
        event Action<SlarkCommand> OnCommand;

        event Action<SlarkNode> OnAttached;
    }
}
