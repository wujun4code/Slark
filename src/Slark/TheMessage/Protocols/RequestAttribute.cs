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

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RpcAttribute : Attribute
    {
        public string Name { get; set; }
        public RpcAttribute(string name)
        {
            Name = name;
        }

        public bool Match(TMJsonRequest request)
        {
            if (string.IsNullOrEmpty(Name)) return false;
            if (!request.Url.StartsWith("/rpc", StringComparison.Ordinal)) return false;

            var result = $"/rpc/{Name}".Equals(request.Url);

            return result;
        }
    }

    public delegate Task RpcFunctionDelegate(SlarkContext context);

    public class StandardRpcFunctionHandler : ISlarkProtocol
    {
        public StandardRpcFunctionHandler(RpcFunctionDelegate funcDelegate)
        {
            FuncDel = funcDelegate;
        }

        public RpcFunctionDelegate FuncDel { get; set; }

        public Task ExecuteAsync(SlarkContext context)
        {
            return FuncDel.Invoke(context);
        }
    }
}
