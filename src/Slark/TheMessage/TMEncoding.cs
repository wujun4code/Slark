using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeanCloud;
using LeanCloud.Core.Internal;
using LeanCloud.Storage.Internal;
using LeanCloud.Utilities;

namespace TheMessage
{
    public class TMEncoding : AVEncoder
    {
        public TMEncoding()
        {

        }

        public static TMEncoding Instance { get; set; } = new TMEncoding();
        public string Serialize(object obj)
        {
            var av = obj as AVObject;
            if (av != null)
            {
                var fullEncoded = av.FullEncode();
                return Serialize(fullEncoded);
            }
            var dict = obj as IDictionary<string, object>;
            if (dict != null)
            {
                return Json.Encode(dict);
            }
            return Convert.ToString(obj, CultureInfo.InvariantCulture);
        }
        internal static readonly string[] DateFormatStrings = {
      // Official ISO format
      "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",

      // It's possible that the string converter server-side may trim trailing zeroes,
      // so these two formats cover ourselves from that.
      "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'",
      "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f'Z'",
    };
        public new object Encode(object obj)
        {
            if (obj is AVObject av)
            {
                return EncodeParseObject(av, false);
            }
            var list = Conversion.As<IList<object>>(obj);
            if (list != null)
            {
                var newArray = new List<object>();
                // We need to explicitly cast `list` to `List<object>` rather than
                // `IList<object>` because IL2CPP is stricter than the usual Unity AOT compiler pipeline.
                if (list.GetType().IsArray)
                {
                    list = new List<object>(list);
                }
                foreach (var item in list)
                {
                    if (!IsValidType(item))
                    {
                        throw new ArgumentException("Invalid type for value in an array");
                    }
                    newArray.Add(Encode(item));
                }
                return newArray;
            }
            if (AVEncoder.IsValidType(obj))
            {
                return base.Encode(obj);
            }
            return obj;
        }

        public object Deserialize(string json)
        {
            return Json.Parse(json);
        }

        public object EncodeProperty(object value)
        {
            return base.Encode(value);
        }

        public IDictionary<string, object> EncodeParseObject(AVObject value, bool isPointer)
        {
            if (isPointer)
            {
                return EncodeParseObject(value);
            }

            var operations = value.GetCurrentOperations();
            var operationJSON = AVObject.ToJSONObjectForSaving(operations);
            var objectJSON = value.ToDictionary(kvp => kvp.Key, kvp => EncodeProperty(kvp.Value));
            foreach (var kvp in operationJSON)
            {
                objectJSON[kvp.Key] = kvp.Value;
            }
            if (value.CreatedAt.HasValue)
            {
                objectJSON["createdAt"] = value.CreatedAt.Value.ToString(DateFormatStrings.First(),
                CultureInfo.InvariantCulture);
            }
            if (value.UpdatedAt.HasValue)
            {
                objectJSON["updatedAt"] = value.UpdatedAt.Value.ToString(DateFormatStrings.First(),
                CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(value.ObjectId))
            {
                objectJSON["objectId"] = value.ObjectId;
            }
            objectJSON["className"] = value.ClassName;
            objectJSON["__type"] = "Object";
            return objectJSON;
        }

        protected override IDictionary<string, object> EncodeParseObject(AVObject value)
        {
            if (value.ObjectId == null)
            {
                return EncodeParseObject(value, false);
            }

            return new Dictionary<string, object> {
                {"__type", "Pointer"},
                { "className", value.ClassName},
                { "objectId", value.ObjectId}
            };
        }
    }
}
