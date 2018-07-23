using System;
using Slark.Core.Utils;

namespace Slark.Core
{
    public class SlarkCorePlugins
    {
        public static SlarkCorePlugins Singleton { get; set; } = new SlarkCorePlugins();

        public SlarkCorePlugins()
        {

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

        private ISlarkEncoder encoder;
        public ISlarkEncoder Encoder
        {
            get
            {
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
                return decoder;
            }
            set
            {
                decoder = value;
            }
        }

    }
}
