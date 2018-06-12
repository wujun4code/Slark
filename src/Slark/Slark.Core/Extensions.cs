using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slark.Core
{
    public static class SlarkExtensions
    {
        public static string ToJsonString(this IDictionary<string, object> source)
        {
            string json = JsonConvert.SerializeObject(source, new JsonSerializerSettings()
            {

            });
            return json;
        }
        public static Task<string> ToJsonStringAsync(this IDictionary<string, object> source)
        {
            return Task.FromResult(source.ToJsonString());
        }
        public static IDictionary<string, object> ToDictionary(this string obj)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
            return values;
        }

        public static Task BroadcastAsync(this IEnumerable<SlarkClientConnection> connections, string message)
        {
            var sendTasks = connections.Select(connection => connection.SendAsync(message));
            return Task.WhenAll(sendTasks);
        }

        public static string RandomOne(this IEnumerable<string> source)
        {
            var rand = new Random();
            var randomOne = source.ElementAt(new System.Random().Next() % source.Count());
            return randomOne;
        }
    }
}
