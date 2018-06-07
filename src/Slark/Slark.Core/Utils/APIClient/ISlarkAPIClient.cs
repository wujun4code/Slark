using System;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkAPIClient
    {
        IEnDecoder EnDecoder { get; set; }

        ISlarkHttpClient HttpClient { get; set; }

        Task<object> GrabAsync(string url);
    }
}
