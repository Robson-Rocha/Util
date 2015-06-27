using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace RobsonROX.Util.FluentProxy
{
    //TODO: [RobsonROX] Adicionar funcionalidade para propriedades: http://www.codeproject.com/Articles/13353/Using-Attributes-and-RealProxy-to-perform-property
    internal class FluentProxyWrapper<T> : RealProxy
    {
        private readonly T _instance;
        private readonly FluentProxy<T> _configuration;

        internal FluentProxyWrapper(T instance, FluentProxy<T> configuration)
        {
            _instance = instance;
            _configuration = configuration;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;

            try
            {
                object[] inArgs = methodCall.InArgs;
                object result;
                FluentProxyActions<T> fluentProxyActions = _configuration.ActionsFor(method.Name);
                if (fluentProxyActions != null)
                {
                    fluentProxyActions.BeforeActions?.Invoke(_instance, method, inArgs);
                    result = (fluentProxyActions.BeforeActions != null) ? fluentProxyActions.InsteadOfActions(_instance, method, inArgs) : method.Invoke(_instance, inArgs);
                    fluentProxyActions.AfterActions?.Invoke(_instance, method, inArgs, result);
                }
                else
                {
                    result = method.Invoke(_instance, inArgs);
                }
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                if (e is TargetInvocationException && e.InnerException != null)
                {
                    return new ReturnMessage(e.InnerException, (IMethodCallMessage) msg);
                }

                return new ReturnMessage(e, (IMethodCallMessage) msg);
            }
        }
    }
}