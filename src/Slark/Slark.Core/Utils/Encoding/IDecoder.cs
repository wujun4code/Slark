using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkDecoder
    {
        Task<object> DecodeAsync(string message);

        T Decode<T>(string message);
    }
}
