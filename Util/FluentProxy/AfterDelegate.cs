using System.Reflection;

namespace RobsonROX.Util.FluentProxy
{
    /// <summary>
    /// Define um m�todo a ser executado antes do m�todo alvo
    /// </summary>
    /// <param name="instance">Inst�ncia real que cont�m o m�todo alvo</param>
    /// <param name="methodInfo">Informa��es sobre a estrutura do m�todo</param>
    /// <param name="args">Par�metros passados para o m�todo</param>
    /// <param name="returnedValue">Valor retornado pelo m�todo alvo</param>
    /// <typeparam name="T">Tipo da inst�ncia real contida no wrapper</typeparam>
    public delegate void AfterDelegate<in T>(T instance, MethodInfo methodInfo, object[] args, object returnedValue);
}