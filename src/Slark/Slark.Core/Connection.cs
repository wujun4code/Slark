using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkClientConnection
    {
        public SlarkClient Client { get; set; }

        public abstract Task SendAsync(string message);
    }
}
