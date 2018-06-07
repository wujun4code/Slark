using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkDecoder
    {
        Task<object> Decode(string message);
    }
}
