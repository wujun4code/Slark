using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkServer
    {
        public SlarkServer()
        {
            ProtocolAdapter = new EchoProtocolAdapter();
            Connections = new List<SlarkClientConnection>();
        }
        public ISlarkProtocolAdapter ProtocolAdapter { get; set; }

        public List<SlarkClientConnection> Connections { get; set; }
    }
}
