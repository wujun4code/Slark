using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface IProtocolMatcher
    {
        Task<ISlarkProtocol> FindAsync(string message);
    }
}
