using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LeanCloud.Core.Internal;
using LeanCloud.Storage.Internal;
using Slark.Core;
using Slark.Core.Protocol;
using TheMessage.Extensions;

namespace TheMessage
{
    public interface IRpc
    {

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
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class RpcHostAttribute : Attribute
    {
        public string Name { get; set; }
        public RpcHostAttribute(string name = null)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class RpcHostIdPropertyAttribute : Attribute
    {
        public RpcHostIdPropertyAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RpcAttribute : Attribute
    {
        public string Name { get; set; }
        public RpcAttribute(string name = null)
        {
            Name = name;
        }
    }

    public static class RpcExtensions
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

        public static void Unregister(this TMLobby server, string functionName)
        {
            server.Unregister(functionName);
        }

        public static TMLobby UseRpc(this IRpc host, TMLobby lobby)
        {
            var tupple = host.ReflectRpcFunctions();
            tupple.ForEach(t =>
            {
                lobby.Register(t.Item1, t.Item2);
            });
            return lobby;
        }

        public static TMLobby RecallRpc(this IRpc host, TMLobby lobby)
        {
            var tupple = host.ReflectRpcFunctions();
            tupple.ForEach(t =>
            {
                lobby.Unregister(t.Item1);
            });
            return lobby;
        }

        public static string ReflectRpcHostIdPropertyValue(this IRpc rpcHost)
        {
            PropertyInfo[] props = rpcHost.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is RpcHostIdPropertyAttribute idAttr)
                    {
                        return prop.GetValue(rpcHost).ToString();
                    }
                }
            }
            return "";
        }

        public static List<(string, RpcFunctionDelegate)> ReflectRpcFunctions(this IRpc rpcHost)
        {
            var hostType = rpcHost.GetType();
            var methods = hostType.GetMethods();

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

                        object host = null;
                        if (!method.IsStatic)
                        {
                            host = rpcHost;
                        }

                        if (method.ReturnType == typeof(Task))
                        {
                            Task awaitable = (Task)method.Invoke(host, pas);
                            await awaitable;
                        }
                        else if (method.ReturnType == typeof(void))
                        {
                            method.Invoke(host, pas);
                        }
                        else if (method.ReturnType.IsGenericType)
                        {
                            if (method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                                result = await method.InvokeAsync(host, pas);
                            else
                            {
                                result = method.Invoke(host, pas);
                            }
                        }
                        else
                        {
                            result = method.Invoke(host, pas);
                        }

                        var encodedObject = TMEncoding.Instance.Encode(result);

                        var resultWrapper = new Dictionary<string, object>()
                        {
                            { "results" , encodedObject }
                        };

                        context.Response = Json.Encode(resultWrapper);
                    };

                    var hostAttribute = hostType.GetCustomAttribute<RpcHostAttribute>();
                    var hostName = string.IsNullOrEmpty(hostAttribute.Name) ? hostType.Name : hostAttribute.Name;
                    var methodName = rpcAttribute.Name ?? method.Name;
                    var rpcMethodName = $"{hostName}_{methodName}";
                    if (!method.IsStatic)
                    {
                        var hostId = rpcHost.ReflectRpcHostIdPropertyValue();
                        rpcMethodName = $"{hostName}_{hostId}_{methodName}";
                    }
                    var mixedResult = (rpcMethodName, rpcFunction);

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
                var pObjs = request.Body["args"] as List<object>;
                for (int i = 0; i < rpcParameters.Length - 1; i++)
                {
                    var pInfo = rpcParameters[i];
                    var pObj = AVDecoder.Instance.Decode(pObjs[i]);
                    var pValue = pObj;
                    result.Add(pValue);
                }
                result.Add(context);
            }
            return result.ToArray();
        }

        public static Task RpcAsync(this IRpc @this, IEnumerable<TMClient> clients, string methodName, bool isStatic, params object[] args)
        {
            var type = @this.GetType();
            var hostAttribute = type.GetCustomAttribute<RpcHostAttribute>();
            var hostName = string.IsNullOrEmpty(hostAttribute.Name) ? type.Name : hostAttribute.Name;
            var rpcMethodName = string.Format("{0}_{1}", hostName, methodName);
            if (!isStatic)
            {
                var hostId = @this.ReflectRpcHostIdPropertyValue();
                rpcMethodName = $"{hostName}_{hostId}_{methodName}";
            }
            return clients.RpcAsync(rpcMethodName, args);
        }

        public static Task RpcAsync(this IRpc @this, IEnumerable<TMClient> clients, string methodName, params object[] args)
        {
            return @this.RpcAsync(clients, methodName, false, args);
        }
    }
}
