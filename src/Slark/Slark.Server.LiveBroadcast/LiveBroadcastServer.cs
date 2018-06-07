using System;
using System.Threading.Tasks;
using Slark.Core;

namespace Slark.Server.LiveBroadcast
{
    public class LiveBroadcastServer : SlarkStandardServer
    {
        public LiveBroadcastServer()
        {
            this.AddPolling(new NBAPollingController(10, 60));
        }
    }
}
