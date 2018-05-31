using System;
using System.Collections.Generic;
using System.Text;

namespace Slark.Core
{
    public abstract class SlarkServerDecorator : SlarkServer
    {
        protected SlarkServer DecoratedServer { get; set; }

        public SlarkServerDecorator(SlarkServer slarkServer) : base()
        {
            this.DecoratedServer = slarkServer;
        }



    }
}
