using System;
namespace Slark.Core.Utils
{
    public class SlarkStandarkEnDecoder : SlarkEnDecoder
    {
        public override ISlarkEncoder Encoder { get; set; }
        public override ISlarkDecoder Decoder { get; set; }

        public SlarkStandarkEnDecoder(ISlarkEncoder encoder, ISlarkDecoder decoder)
        {
            Encoder = encoder;
            Decoder = decoder;
        }
    }
}
