using System;
using System.Collections.Generic;
using System.Text;

namespace Slark.Core
{
    public abstract class SlarkClient
    {
        public SlarkClient()
        {

        }
        public SlarkToken Token { get; set; }
    }
}
