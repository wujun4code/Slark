using System;
using System.Collections.Generic;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayContext : SlarkContext
    {
        public SlarkContext coreContext;

        public PlayContext(SlarkContext context)
        {
            coreContext = context;
        }

        public override SlarkServer Server { get => coreContext.Server; set => coreContext.Server = value; }
        public override SlarkClientConnection Sender { get => coreContext.Sender; set => coreContext.Sender = value; }
        public override ISlarkMessage Message { get => coreContext.Message; set => coreContext.Message = value; }
        public override string Response { get => coreContext.Response; set => coreContext.Response = value; }
        public override string Notice { get => coreContext.Notice; set => coreContext.Notice = value; }
        public override IEnumerable<SlarkClientConnection> Receivers { get => coreContext.Receivers; set => coreContext.Receivers = value; }
        public override bool HasNotice { get => coreContext.HasNotice; set => coreContext.HasNotice = value; }
    }

}
