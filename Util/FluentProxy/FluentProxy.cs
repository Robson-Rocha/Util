using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RobsonROX.Util.FluentProxy
{
    /// <summary>
    /// Cria um proxy configurável para um objeto
    /// </summary>
    /// <typeparam name="T">Tipo real para o qual será criado o proxy</typeparam>
    public sealed class FluentProxy<T> : IFluentProxyExecuters<T>
    {
        private readonly MethodInfo[] _methods;
        private readonly Dictionary<string, FluentProxyActions<T>> _actions;
        private string _currentMethod;

        /// <summary>
        /// Construtor.
        /// </summary>
        public FluentProxy()
        {
            _methods = typeof(T).GetMethods();
            _actions = new Dictionary<string, FluentProxyActions<T>>();
        }

        internal FluentProxyActions<T> ActionsFor(string memberName)
        {
            return _actions.ContainsKey(memberName) ? _actions[memberName] : null;
        }

        /// <summary>
        /// Define o método a ser configurado
        /// </summary>
        /// <param name="methodName">Nome do método</param>
        /// <returns>Esta instância</returns>
        public FluentProxy<T> For(string methodName)
        {
            if (_methods.Any(m => m.Name == methodName))
            {
                var fluentProxyActions = new FluentProxyActions<T>();
                _actions.Add(methodName, fluentProxyActions);
                _currentMethod = methodName;
            }

            return this;
        }

        private void CheckIfHasCurrentMethod()
        {
            if (_currentMethod == null) throw new NullReferenceException("Defina o método antes de definir uma ação.");
        }

        /// <summary>
        /// Remove quaisquer configurações para o método atual
        /// </summary>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> IFluentProxyExecuters<T>.Clear()
        {
            CheckIfHasCurrentMethod();
            _actions[_currentMethod] = new FluentProxyActions<T>();
            return this;
        }

        /// <summary>
        /// Define o método a ser executado antes do método atual
        /// </summary>
        /// <param name="beforeAction">Método a ser executado antes do método atual</param>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> IFluentProxyExecuters<T>.ExecuteBefore(BeforeDelegate<T> beforeAction)
        {
            CheckIfHasCurrentMethod();
            _actions[_currentMethod].BeforeActions += beforeAction;
            return this;
        }

        /// <summary>
        /// Define o método a ser executado depois do método atual
        /// </summary>
        /// <param name="insteadOfAction">Método a ser executado depois do método atual</param>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> IFluentProxyExecuters<T>.ExecuteInstead(InsteadOfDelegate<T> insteadOfAction)
        {
            CheckIfHasCurrentMethod();
            _actions[_currentMethod].InsteadOfActions += insteadOfAction;
            return this;
        }

        /// <summary>
        /// Define o método a ser executado ao invés do método atual
        /// </summary>
        /// <param name="afterAction">Método a ser executado ao invés do método atual</param>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> IFluentProxyExecuters<T>.ExecuteAfter(AfterDelegate<T> afterAction)
        {
            CheckIfHasCurrentMethod();
            _actions[_currentMethod].AfterActions += afterAction;
            return this;
        }

        /// <summary>
        /// Cria um proxy para a instância da classe informada
        /// </summary>
        /// <param name="instance">Instância a ser envolvida no proxy</param>
        /// <returns>Proxy envolvendo a instância informada</returns>
        public T CreateProxy(T instance)
        {
            return (T)new FluentProxyWrapper<T>(instance, this).GetTransparentProxy();
        }
    }
}