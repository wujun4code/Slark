using System;
using System.Threading.Tasks;
using System.Threading;

namespace Slark.Core.Utils
{
    public abstract class SlarkPollingController
    {
        public uint IntervalInSecond { get; set; }

        public uint DueTimeInSecond { get; set; }

        public SlarkServer Server { get; set; }

        private Timer ticker;

        public SlarkPollingController()
            : this(0, 60)
        {

        }

        public SlarkPollingController(uint dueTimeInSecond, uint intervalInSecond)
        {
            DueTimeInSecond = dueTimeInSecond;
            IntervalInSecond = intervalInSecond;
            ticker = new Timer(TimerMethod, this, 1000 * DueTimeInSecond, 1000 * intervalInSecond);
        }

        public virtual async void TimerMethod(object state)
        {
            var message = await PollingAsync();
            await this.Server.BroadcastAsync(message);
        }

        public abstract Task<string> PollingAsync();
    }
}
