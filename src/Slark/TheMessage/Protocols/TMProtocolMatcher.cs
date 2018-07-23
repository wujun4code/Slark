using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TheMessage.Protocols
{
    public class TMProtocolMatcher : SlarkStandardProtocolMatcher
    {
        public override Task<ISlarkProtocol> FindAsync(SlarkContext context)
        {
            if (context.Message is TMJsonRequest request)
            {
                if (request != null)
                {
                    var methods = context.Server.GetType().GetMethods();

                    foreach (var method in methods)
                    {
                        var requestAttributes = method.GetCustomAttributes(typeof(RequestAttribute), false);
                        if (requestAttributes.Length == 1)
                        {
                            var functionAttribute = (RequestAttribute)requestAttributes[0];
                            if (functionAttribute.Match(request))
                            {
                                RpcFunctionDelegate del = method.IsStatic ? (RpcFunctionDelegate)Delegate.CreateDelegate(typeof(RpcFunctionDelegate), method) : (RpcFunctionDelegate)Delegate.CreateDelegate(typeof(RpcFunctionDelegate), context.Server, method);
                                return Task.FromResult(new StandardRpcFunctionHandler(del) as ISlarkProtocol);
                            }
                        }
                        var rpcAttributes = method.GetCustomAttributes(typeof(RpcAttribute), false);

                        if (requestAttributes.Length == 1)
                        {
                            var rpcAttribute = (RpcAttribute)rpcAttributes[0];
                            if (rpcAttribute.Match(request))
                            {
                                var pa = request.Body["p"] as List<object>;

                            }
                        }
                    }
                }
            }

            return base.FindAsync(context);
        }
    }
}
