using System;
namespace Slark.Core.Utils
{
    public class SlarkStandardAPIClient : SlarkAPIClient
    {
        public SlarkStandardAPIClient(IEnDecoder endecoder, ISlarkHttpClient httpClient)
        {
            EnDecoder = endecoder;
            HttpClient = httpClient;
        }

        public override IEnDecoder EnDecoder { get; set; }
        public override ISlarkHttpClient HttpClient { get; set; }
    }

}