using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanCloud.Storage.Internal;
using Slark.Core.Utils;

namespace TheMessage
{
    public class LeanCloudJsonEncoder : ISlarkEncoder
    {
        public string Encode(object obj)
        {
            string json = Json.Encode(obj);
            return json;
        }

        public Task<string> EncodeAsync(object obj)
        {
            return Task.FromResult(Encode(obj));
        }
    }
}
