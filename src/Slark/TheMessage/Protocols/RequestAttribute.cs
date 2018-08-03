using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace TheMessage.Protocols
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RequestAttribute : Attribute
    {
        public string Url { get; set; }

        public string Method { get; set; } = "GET";

        public RequestAttribute(string url, string method = null)
        {
            Url = url;
            if (!string.IsNullOrEmpty(method)) Method = method;
        }

        public bool Match(TMJsonRequest request)
        {
            if (Url.StartsWith("/rpc", StringComparison.Ordinal) && request.Url.StartsWith("/rpc", StringComparison.Ordinal))
            {
                return true;
            }

            if (string.IsNullOrEmpty(Url)) return false;

            var result = Url.Equals(request.Url);

            if (!string.IsNullOrEmpty(request.Method))
            {
                result &= Method.ToLower().Equals(request.Method.ToLower());
            }
            return result;
        }
    }
}
