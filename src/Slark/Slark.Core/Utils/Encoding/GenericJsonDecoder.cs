using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slark.Core.Utils
{
    public class SlarkGenericJsonDecoder<T> : ISlarkDecoder
    {
        public async Task<object> Decode(string message)
        {
            var obj = await this.DecodeAsync(message);
            return obj;
        }

        public Task<T> DecodeAsync(string message)
        {
            return Task.FromResult(JsonConvert.DeserializeObject<T>(message));
        }
    }
}
