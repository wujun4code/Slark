using System;
using System.Collections.Generic;
using System.Text;

namespace Slark.Core
{
    public struct SlarkConnectionConfig
    {
        public IEnumerable<string> Services { get; set; }

        public SlarkConnectionConfig Default
        {
            get
            {
                return new SlarkConnectionConfig();
            }
        }
    }
}
