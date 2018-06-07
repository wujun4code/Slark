using System;
using System.Net;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public abstract class SlarkHttpClient : ISlarkHttpClient
    {
        public abstract Task<string> GetAsync(string url);
    }
}
