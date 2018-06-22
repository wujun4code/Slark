using System;
using System.Collections.Generic;
using Slark.Core.Protocol;

namespace Slark.Core
{
    public class StandardContext : SlarkContext
    {
        public StandardContext()
        {
        }

        public override SlarkServer Server { get; set; }
        public override SlarkClientConnection Sender { get; set; }
        public override ISlarkMessage Message { get; set; }
        public override string Response { get; set; }
        public override string Notice { get; set; }
        public override IEnumerable<SlarkClientConnection> Receivers { get; set; }
        public override bool HasNotice { get; set; }
    }
}
