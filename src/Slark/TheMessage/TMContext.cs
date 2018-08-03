using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace TheMessage
{
    public class TMContext : SlarkContext
    {
        public TMContext()
        {

        }

        public TMClient Client
        {
            get
            {
                return Sender.Client as TMClient;
            }
        }

        public override SlarkServer Server { get; set; }
        public override SlarkClientConnection Sender { get; set; }
        public override ISlarkMessage Message { get; set; }
        public override string Response { get; set; }
        public override string Notice { get; set; }
        public override IEnumerable<SlarkClientConnection> Receivers { get; set; }
        public override bool HasNotice { get; set; }

        public override Task ReplyAsync()
        {
            if (string.IsNullOrEmpty(Response)) Response = "{}";
            if (Message is TMJsonRequest request)
            {
                var resJson = Response.ToDictionary();
                resJson["i"] = request.CommandId;
                Response = resJson.ToJsonString();
            }
            return base.ReplyAsync();
        }

        public Task ReplyOKAsync()
        {
            var result = new Dictionary<string, object>()
            {
                { "results", "ok" }
            };

            Response = result.ToJsonString();
            return ReplyAsync();
        }
    }
}
