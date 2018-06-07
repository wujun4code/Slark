using System;
using Slark.Core.Utils;

namespace Slark.Core
{
    public class SlarkCorePlugins
    {
        public static SlarkCorePlugins Singleton { get; set; }

        public SlarkCorePlugins()
        {

        }

        private ISlarkAPIClient apiClient;
        public ISlarkAPIClient APIClient
        {
            get
            {
                if (apiClient == null) apiClient = new SlarkStandardAPIClient(this.EnDecoder, this.httpClient);
                return apiClient;
            }
            set
            {
                apiClient = value;
            }
        }


        private ISlarkHttpClient httpClient;
        public ISlarkHttpClient HttpClient
        {
            get
            {
                if (httpClient == null) httpClient = new SlarkStandardHttpClient();
                return httpClient;
            }
            set
            {
                httpClient = value;
            }
        }

        private SlarkStandarkEnDecoder endecoder;
        public SlarkStandarkEnDecoder EnDecoder
        {
            get
            {
                if (endecoder == null) endecoder = new SlarkStandarkEnDecoder(this.Encoder, this.Decoder);
                return endecoder;
            }
            set
            {
                endecoder = value;
            }
        }

        private ISlarkEncoder encoder;
        public ISlarkEncoder Encoder
        {
            get
            {
                if (encoder == null) encoder = new JsonEncoder();
                return encoder;
            }
            set
            {
                encoder = value;
            }
        }

        private ISlarkDecoder decoder;
        public ISlarkDecoder Decoder
        {
            get
            {
                if (decoder == null) decoder = new JsonDecoder();
                return decoder;
            }
            set
            {
                decoder = value;
            }
        }

    }
}
