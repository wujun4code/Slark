using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeanCloud;
using LeanCloud.Core.Internal;
using LeanCloud.Storage.Internal;

namespace TheMessage
{
    public static class GlobalExtensions
    {

        public static void SetTimeout<T>(this TaskCompletionSource<T> tcs, int miliseconds = 5000)
        {
            var ct = new CancellationTokenSource(miliseconds);
            ct.Token.Register(() =>
            {
                tcs.TrySetCanceled();
            }, false);
        }

        public static void SetDefaultResult<T>(this TaskCompletionSource<T> tcs, T result, int miliseconds = 5000, Action clean = null)
        {
            var ct = new CancellationTokenSource(miliseconds);
            ct.Token.Register(() =>
            {
                tcs.TrySetResult(result);
            }, false);
        }

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
            if (!string.IsNullOrEmpty(obj.ObjectId))
            {
                objectJSON["objectId"] = obj.ObjectId;
            }
            objectJSON["__type"] = "Object";
            return objectJSON;
        }

    }
}
