using System;
using System.Threading.Tasks;
using LeanCloud.Storage.Internal;
using Slark.Core.Utils;

namespace TheMessage
{
    public class LeanCloudJsonDecoder : ISlarkDecoder
    {
        public T Decode<T>(string message)
        {
            var obj = Json.Parse(message);
            return (T)obj;
        }

        public Task<object> DecodeAsync(string message)
        {
            var obj = Json.Parse(message);
            return Task.FromResult(obj);
        }
    }
}
