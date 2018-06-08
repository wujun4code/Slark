using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public abstract class SlarkAPIClient : ISlarkAPIClient
    {
        public abstract IEnDecoder EnDecoder { get; set; }
        public abstract ISlarkHttpClient HttpClient { get; set; }

        public virtual async Task<string> GrabAsync(string url)
        {
            var response = await HttpClient.GetAsync(url);
            var obj = await EnDecoder.Decoder.Decode(response);
            var text = await EnDecoder.Encoder.Encode(obj);
            return text;
        }
    }
}
