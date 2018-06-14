using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slark.Server.LeanCloud.Play
{
    public static class PlayExtensions
    {
        public static IDictionary<K, V> ToDictionary<K, V>(this Hashtable table)
        {
            return table
              .Cast<DictionaryEntry>()
              .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
        }

        public static bool ValueEquals(this Hashtable source, Hashtable toCompare)
        {
            bool same = source.Cast<DictionaryEntry>().Union(toCompare.Cast<DictionaryEntry>()).Count() == source.Count;
            return same;
        }

        public static Hashtable AutomicSet(this Hashtable source, Hashtable toSet)
        {
            lock (source.SyncRoot)
            {
                var updatedOrAdd = new Hashtable();
                var tableDictionary = toSet.ToDictionary<object, object>();
                foreach (var tuoa in tableDictionary)
                {
                    source[tuoa.Key] = tuoa.Value;
                    updatedOrAdd[tuoa.Key] = tuoa.Value;
                }
                return updatedOrAdd;
            }
        }

        public static Hashtable AutomicUpdateOrAdd(this Hashtable source, IDictionary<string, PlayCAS> toUpdateOrAdd)
        {
            lock (source.SyncRoot)
            {
                var updatedOrAdd = new Hashtable();
                foreach (var tuoa in toUpdateOrAdd)
                {
                    var hasKey = source.ContainsKey(tuoa.Key);
                    var expectedDismatch = source[tuoa.Key].ToString() != tuoa.Value.ExpectedValue.ToString();

                    if (hasKey && expectedDismatch)
                    {
                        continue;
                    }

                    source[tuoa.Key] = tuoa.Value.ValueToSet;
                    updatedOrAdd[tuoa.Key] = tuoa.Value.ValueToSet;
                }
                return updatedOrAdd;
            }
        }

        public class PlayCAS
        {
            public object ExpectedValue { get; set; }
            public object ValueToSet { get; set; }
        }
    }
}
