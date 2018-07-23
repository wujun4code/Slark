using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Slark.Core;
using Slark.Core.Utils.Protocol;
using TheMessage.Protocols;

namespace TheMessage.Extensions
{
    public static class TheMessageExtensions
    {
        public static void Register(this TMLobby server, string functionName, RpcFunctionDelegate rpcFunction)
        {
            server.Register(functionName, new StandardRpcFunctionHandler(rpcFunction));
        }

        public static TMLobby UseRpc(this TMLobby server)
        {
            var tupple = server.ReflectFunctions();
            tupple.ForEach(t =>
            {
                server.Register(t.Item1, t.Item2);
            });
            return server;
        }
        public static List<(string, RpcFunctionDelegate)> ReflectFunctions(this TMLobby server)
        {
            var methods = server.GetType().GetMethods();

            var tupple = new List<(string, RpcFunctionDelegate)>();
            foreach (var method in methods)
            {
                var hookAttributes = method.GetCustomAttributes(typeof(RpcAttribute), false);

                if (hookAttributes.Length == 1)
                {
                    var functionAttribute = (RpcAttribute)hookAttributes[0];

                    RpcFunctionDelegate rpcFunction = (context) =>
                    {
                        var pas = BindParamters(method, context);
                        if (method.IsStatic)
                        {
                            method.Invoke(null, pas);
                        }
                        else
                        {
                            method.Invoke(server, pas);
                        }
                        return Task.FromResult(context);
                    };
                    var mixedResult = (functionAttribute.Name, rpcFunction);

                    tupple.Add(mixedResult);
                }
            }
            return tupple;
        }
        public static object[] BindParamters(MethodInfo memberInfo, SlarkContext context)
        {
            List<object> result = new List<object>();
            ParameterInfo[] rpcParameters = memberInfo.GetParameters();
            if (context.Message is TMJsonRequest request)
            {
                var pObjs = request.Body["p"] as JArray;
                for (int i = 0; i < rpcParameters.Length; i++)
                {
                    var pInfo = rpcParameters[i];
                    var pValue = pObjs[i].ToObject(pInfo.ParameterType);
                    result.Add(pValue);
                }
            }
            return result.ToArray();
        }
    }
}
