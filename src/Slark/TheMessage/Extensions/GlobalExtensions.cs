using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheMessage
{
    public static class GlobalExtensions
    {
        public static string ToTMJsonString(this IDictionary<string, object> source)
        {
            string json = JsonConvert.SerializeObject(source);
            return json;
        }

        public static IDictionary<string, object> ToTMDictionary(this string obj)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
            return values;
        } 
    }
}
