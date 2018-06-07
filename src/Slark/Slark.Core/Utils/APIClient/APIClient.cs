using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public abstract class SlarkAPIClient : ISlarkAPIClient
    {
        public abstract IEnDecoder EnDecoder { get; set; }
        public abstract ISlarkHttpClient HttpClient { get; set; }

        public SlarkAPIClient()
        {

        }

        public async Task<object> GrabAsync(string url)
        {
            var response = await HttpClient.GetAsync(url);
            var obj = await EnDecoder.Decoder.Decode(response);
            return obj;
        }
    }
}
