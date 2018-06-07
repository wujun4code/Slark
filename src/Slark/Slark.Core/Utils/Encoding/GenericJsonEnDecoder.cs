using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slark.Core.Utils
{
    public class SlarkGenericJsonEnDecoder<T> : IEnDecoder
    {
        public SlarkGenericJsonEnDecoder()
        {
            Decoder = new SlarkGenericJsonDecoder<T>();
            Encoder = new SlarkGenericJsonEncoder<T>();
        }

        public ISlarkEncoder Encoder { get; set; }
        public ISlarkDecoder Decoder { get; set; }

    }
}
