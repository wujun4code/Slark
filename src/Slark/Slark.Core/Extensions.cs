using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime;
using System.Collections;

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

        public static T RandomOne<T>(this IEnumerable<T> source)
        {
            var rand = new Random();
            var randomOne = source.ElementAt(new System.Random().Next() % source.Count());
            return randomOne;
        }

        public static bool TryGet<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T result)
        {
            result = default(T);
            var wts = source.Where(predicate);
            result = wts.FirstOrDefault();
            return wts.Any();
        }

        public static KeyValuePair<TKey, TValue> GetEntry<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
        }

        public enum UnixTimeStampUnit
        {
            Second = 1,
            Milisecond = 1000,
        }

        public static long ToUnixTimeStamp(this DateTime date, UnixTimeStampUnit unit = UnixTimeStampUnit.Milisecond)
        {
            long unixTimestamp = (long)(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return (unixTimestamp * (int)unit);
        }

        public static DateTime ToDateTime(this long timestamp, UnixTimeStampUnit unit = UnixTimeStampUnit.Milisecond)
        {
            var timespan = timestamp * 1000 / (int)(unit);
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(timespan).ToLocalTime();
            return dtDateTime;
        }

    }
}