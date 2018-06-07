using System;
namespace Slark.Core.Utils
{
    public abstract class SlarkEnDecoder : IEnDecoder
    {
        public abstract ISlarkEncoder Encoder { get; set; }
        public abstract ISlarkDecoder Decoder { get; set; }
    }
}
