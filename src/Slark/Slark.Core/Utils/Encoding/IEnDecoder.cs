using System;
namespace Slark.Core.Utils
{
    public interface IEnDecoder
    {
        ISlarkEncoder Encoder { get; set; }
        ISlarkDecoder Decoder { get; set; }
    }
}
