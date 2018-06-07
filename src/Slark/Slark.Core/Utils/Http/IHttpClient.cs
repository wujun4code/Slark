using System;
using System.Net;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkHttpClient
    {
        Task<string> GetAsync(string url);
    }
}
