using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slark.Core.Utils
{
    public class JsonEncoder : ISlarkEncoder
    {

        public Task<string> Encode(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {

            });
            return Task.FromResult(json);
        }
    }
}
