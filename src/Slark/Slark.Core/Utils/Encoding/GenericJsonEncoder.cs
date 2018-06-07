using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slark.Core.Utils
{
    public class SlarkGenericJsonEncoder<T> : ISlarkEncoder
    {
        public SlarkGenericJsonEncoder()
        {

        }

        public Task<string> EncodeAsync(object obj)
        {
            return Task.FromResult(JsonConvert.SerializeObject(obj));
        }

        public Task<string> Encode(object obj)
        {
            return this.EncodeAsync(obj);
        }
    }
}
