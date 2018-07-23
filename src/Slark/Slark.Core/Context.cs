using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core.Protocol;

namespace Slark.Core
{
    public abstract class SlarkContext
    {
        public abstract SlarkServer Server { get; set; }

        public abstract SlarkClientConnection Sender { get; set; }

        public abstract ISlarkMessage Message { get; set; }

        public abstract string Response { get; set; }

        public abstract string Notice { get; set; }

        public abstract IEnumerable<SlarkClientConnection> Receivers { get; set; }

        public virtual Task PushNoticeAsync()
        {
            return Receivers.BroadcastAsync(Notice);
        }

        public virtual Task ReplyAsync()
        {
            if (string.IsNullOrEmpty(Response))
            {
                Response = "";
            }
            return Sender.SendAsync(Response);
        }

        public abstract bool HasNotice
        {
            get; set;
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