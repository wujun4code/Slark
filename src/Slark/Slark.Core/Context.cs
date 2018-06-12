using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core.Protocol;

namespace Slark.Core
{
    public class SlarkContext
    {
        public SlarkServer Server { get; set; }

        public SlarkClientConnection Sender { get; set; }

        public ISlarkMessage Message { get; set; }

        public string Response { get; set; }

        public string Notice { get; set; }

        public IEnumerable<SlarkClientConnection> Receivers { get; set; }

        public virtual Task PushNoticeAsync()
        {
            return Receivers.BroadcastAsync(Notice);
        }

        public virtual Task ReplyAsync()
        {
            return Sender.SendAsync(Response);
        }

        public bool HasNotice
        {
            get;set;
        }
    }

    public static class SlarkContextExtenions
    {
        public static IEnumerable<SlarkClientConnection> ToEnumerable(this SlarkClientConnection connection)
        {
            return new SlarkClientConnection[] { connection };
        }

        public static Task<IEnumerable<SlarkClientConnection>> ToEnumerableAsync(this SlarkClientConnection connection)
        {
            return Task.FromResult(connection.ToEnumerable());
        }
    }

}