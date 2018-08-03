using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;
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
                    }
                }
            }
            else if (context.Message is TMJsonResponse response)
            {
                if (context.Sender.Client is TMClient client)
                {
                    client.OnResponse?.Invoke(client, response);
                }

                return Task.FromResult(new OkProtocol() as ISlarkProtocol);
            }

            return base.FindAsync(context);
        }
    }
}
