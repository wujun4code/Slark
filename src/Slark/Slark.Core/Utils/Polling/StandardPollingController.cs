using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public class SlarkStandardPollingController : SlarkPollingController
    {
        public string APIUrl { get; set; }

        public ISlarkAPIClient APIClient { get; set; }

        public SlarkStandardPollingController(string apiUrl, ISlarkAPIClient apiClient, uint dueTimeInSecond, uint intervalInSecond)
            : base(dueTimeInSecond, intervalInSecond)
        {
            APIUrl = apiUrl;
            APIClient = apiClient;
        }

        public override async Task<string> PollingAsync()
        {
            var text = await APIClient.GrabAsync(APIUrl);
            return text;
        }
    }
}
