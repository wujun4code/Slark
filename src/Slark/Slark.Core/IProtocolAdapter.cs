using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Core
{
    public interface ISlarkProtocolAdapter
    {
        Task OnConnected(SlarkClientConnection slarkClientConnection);

        Task OnReceived(SlarkClientConnection slarkClientConnection, string message);

        Task OnDisconnected(SlarkClientConnection slarkClientConnection);
    }
}
