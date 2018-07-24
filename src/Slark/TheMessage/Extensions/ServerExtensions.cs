using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LeanCloud;
using LeanCloud.Core.Internal;
using LeanCloud.Storage.Internal;
using Slark.Core;
using Slark.Core.Utils.Protocol;
using TheMessage.Protocols;

namespace TheMessage.Extensions
{
    public static class TheMessageExtensions
    {
        public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }
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
                    var rpcAttribute = (RpcAttribute)hookAttributes[0];

                    RpcFunctionDelegate rpcFunction = async (context) =>
                    {
                        var pas = BindParamters(method, context);

                        object result = null;

                        if (method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            if (method.IsStatic)
                            {
                                result = await method.InvokeAsync(null, pas);
                            }
                            else
                            {
                                result = await method.InvokeAsync(server, pas);
                            }
                        }
                        else
                        {
                            if (method.IsStatic)
                            {
                                result = method.Invoke(null, pas);
                            }
                            else
                            {
                                result = method.Invoke(server, pas);
                            }
                        }

                        var encodedObject = RpcEncode(result);

                        var resultWrapper = new Dictionary<string, object>()
                        {
                            { "results" , encodedObject }
                        };

                        context.Response = Json.Encode(resultWrapper);
                    };

                    var rpcMethodName = rpcAttribute.Name ?? method.Name;
                    var mixedResult = (rpcMethodName, rpcFunction);

                    tupple.Add(mixedResult);
                }
            }
            return tupple;
        }

        public static object RpcEncode(object result)
        {
            if (result is AVObject avobj)
            {
                var encodeObj = avobj.FullEncode();
                return encodeObj;
            }
            return result;
        }

        public static object[] BindParamters(MethodInfo memberInfo, SlarkContext context)
        {
            List<object> result = new List<object>();
            ParameterInfo[] rpcParameters = memberInfo.GetParameters();
            if (context.Message is TMJsonRequest request)
            {
                var pObjs = request.Body["args"] as List<object>;
                for (int i = 0; i < rpcParameters.Length; i++)
                {

                    var pInfo = rpcParameters[i];
                    var pValue = Convert.ChangeType(pObjs[i], pInfo.ParameterType);
                    result.Add(pValue);
                }
            }
            return result.ToArray();
        }
    }
}
