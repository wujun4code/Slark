using System;
using System.Threading.Tasks;
using Slark.Core.Utils;
using Slark.Server.LiveBroadcast.NBA;

namespace Slark.Server.LiveBroadcast
{
    public class NBAPollingController : SlarkGenericJsonPollingController<Game>
    {
        public NBAPollingController(string apiUrl, uint dueTimeInSecond, uint intervalInSecond)
            : base(apiUrl, dueTimeInSecond, intervalInSecond)
        {
            this.APIClient.EnDecoder.Decoder = new Game();
        }

        public NBAPollingController(uint dueTimeInSecond = 0, uint intervalInSecond = 60)
            : this(string.Format("http://data.nba.net/prod/v1/{0}/scoreboard.json", DateTime.Now.AddDays(-1).ToString("yyyyMMdd")), dueTimeInSecond, intervalInSecond)
        {

        }
    }
}
