using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slark.Core.Utils
{
    public class JsonDecoder : ISlarkDecoder
    {
        public Task<object> Decode(string message)
        {
            var obj = JsonConvert.DeserializeObject(message);
            return Task.FromResult(obj);
        }
    }
}
