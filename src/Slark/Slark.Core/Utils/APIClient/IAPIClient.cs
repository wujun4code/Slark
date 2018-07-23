using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkAPIClient
    {
        ISlarkDecoder Decoder { get; set; }

        ISlarkEncoder Encoder { get; set; }

        ISlarkHttpClient HttpClient { get; set; }

        Task<string> GrabAsync(string url);
    }
}
