using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public abstract class SlarkAPIClient : ISlarkAPIClient
    {
        public virtual ISlarkHttpClient HttpClient { get; set; }
        public virtual ISlarkDecoder Decoder { get; set; }
        public virtual ISlarkEncoder Encoder { get; set; }

        public virtual async Task<string> GrabAsync(string url)
        {
            var response = await HttpClient.GetAsync(url);
            var obj = await Decoder.DecodeAsync(response);
            var text = await Encoder.EncodeAsync(obj);
            return text;
        }
    }
}
