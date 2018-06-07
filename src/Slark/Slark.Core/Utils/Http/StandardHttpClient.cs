using System;
using System.Net;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public class SlarkStandardHttpClient : SlarkHttpClient
    {
        private WebClient client = new WebClient();
        public override Task<string> GetAsync(string url)
        {
            var uri = new Uri(url);
            return client.DownloadStringTaskAsync(uri);
        }
    }
}
