using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slark.ClassesDefinition
{
    public static class SlarkExtensions
    {
        public static string ToJsonString(this IDictionary<string, object> source)
        {
            string json = JsonConvert.SerializeObject(source, Formatting.Indented);
            return json;
        }

        public static IDictionary<string, object> ToDictionary(this string obj)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
            return values;
        }
    }
}
