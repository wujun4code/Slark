using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeanCloud;
using LeanCloud.Core.Internal;
using LeanCloud.Storage.Internal;

namespace TheMessage
{
    public static class GlobalExtensions
    {
        internal static readonly string[] DateFormatStrings = {
      // Official ISO format
      "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",

      // It's possible that the string converter server-side may trim trailing zeroes,
      // so these two formats cover ourselves from that.
      "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'",
      "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f'Z'",
    };
        public static IDictionary<string, object> FullEncode(this AVObject obj)
        {
            var objectJSON = obj.Encode();
            if (obj.CreatedAt.HasValue)
            {
                objectJSON["createdAt"] = obj.CreatedAt.Value.ToString(DateFormatStrings.First(),
                CultureInfo.InvariantCulture);
            }
            if (obj.UpdatedAt.HasValue)
            {
                objectJSON["updatedAt"] = obj.UpdatedAt.Value.ToString(DateFormatStrings.First(),
                CultureInfo.InvariantCulture);
            }
            objectJSON["className"] = obj.ClassName;

            return objectJSON;
        }

    }
}
