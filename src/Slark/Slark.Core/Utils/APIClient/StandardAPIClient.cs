using System;
namespace Slark.Core.Utils
{
    public class SlarkStandardAPIClient : SlarkAPIClient
    {
        public SlarkStandardAPIClient(ISlarkDecoder decoder, ISlarkEncoder encoder, ISlarkHttpClient httpClient)
        {
            Decoder = decoder;
            Encoder = encoder;
            HttpClient = httpClient;
        }
    }

}