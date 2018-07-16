using System;
namespace Slark.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class InjectiveAttribute : Attribute
    {
        public InjectiveAttribute(string methodName)
        {
            ReplacedMethodName = methodName;
        }

        public string ReplacedMethodName { get; set; }
    }

    public static class DelegateInjectionExtensions
    {
        public static SlarkStandardServer Inject(this SlarkStandardServer server, CreateClientAsyncFunc del)
        {
            server.CreateClientAsync = del;
            return server;
        }

        public static SlarkStandardServer Inject<T>(this SlarkStandardServer server, T tInstance = null) where T : class
        {
            var methods = tInstance != null ? tInstance.GetType().GetMethods() : typeof(T).GetMethods();
            foreach (var method in methods)
            {
                var injectiveAttributes = method.GetCustomAttributes(typeof(InjectiveAttribute), false);
                if (injectiveAttributes.Length == 1)
                {
                    var functionAttribute = (InjectiveAttribute)injectiveAttributes[0];
                    if (functionAttribute.ReplacedMethodName.ToLower() == "createclientasync")
                    {
                        CreateClientAsyncFunc del = method.IsStatic ? (CreateClientAsyncFunc)Delegate.CreateDelegate(typeof(CreateClientAsyncFunc), method) : (CreateClientAsyncFunc)Delegate.CreateDelegate(typeof(CreateClientAsyncFunc), tInstance ?? Activator.CreateInstance<T>(), method);
                        server.Inject(del);
                    }
                }
            }
            return server;
        }
    }
}
