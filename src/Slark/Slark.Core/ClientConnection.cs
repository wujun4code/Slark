using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkClientConnection
    {
        public string Id { get; set; }

        public SlarkClient Client { get; set; }

        public abstract Task SendAsync(string message);
    }
}
