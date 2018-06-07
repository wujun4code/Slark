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
            var response = await APIClient.HttpClient.GetAsync(APIUrl);
            var obj = await APIClient.EnDecoder.Decoder.Decode(response);
            var text = await APIClient.EnDecoder.Encoder.Encode(obj);
            return text;
        }
    }
}
