using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;
using Newtonsoft.Json;
namespace TheMessage.Protocols
{
    public delegate Task ProcessAsync(SlarkContext context);

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
                                ProcessAsync del = method.IsStatic ? (ProcessAsync)Delegate.CreateDelegate(typeof(ProcessAsync), method) : (ProcessAsync)Delegate.CreateDelegate(typeof(ProcessAsync), context.Server, method);
                                return Task.FromResult(new TMSlarkProtocol(del) as ISlarkProtocol);
                            }
                        }
                    }
                }
            }

            return base.FindAsync(context);
        }
    }
}
