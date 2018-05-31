using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkClientConnection
    {
        public SlarkConnectionConfig Config { get; set; }
        public SlarkClient Client { get; set; }

        public Guid ServerId
        {
            get
            {
                return Client.ServerSetId;
            }
        }

        public abstract Task SendAsync(string message);
    }
}
