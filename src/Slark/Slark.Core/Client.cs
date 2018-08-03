using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Core
{
    public abstract class SlarkClient
    {
        public SlarkToken Token { get; set; }

        public Func<string, Task> Courier { get; set; }

        public virtual Task SendAsync(string message)
        {
            return Courier(message);
        }
    }
}
