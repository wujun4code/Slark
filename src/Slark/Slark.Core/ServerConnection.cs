using System;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkServerConnection
    {
        public string Id { get; set; }

        public SlarkServer Server { get; set; }

        public abstract Task<string> RPCAsync(string message);
    }
}
