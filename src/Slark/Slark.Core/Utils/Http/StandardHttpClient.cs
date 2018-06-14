using System;
using System.Net;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public class SlarkStandardHttpClient : SlarkHttpClient
    {
        private readonly WebClient client = new WebClient();

        public WebClient Client => client;

        public override Task<string> GetAsync(string url)
        {
            var uri = new Uri(url);
            return Client.DownloadStringTaskAsync(uri);
        }
    }
}
