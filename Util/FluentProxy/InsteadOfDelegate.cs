using System.Reflection;

namespace RobsonROX.Util.FluentProxy
{
    /// <summary>
    /// Define um m�todo a ser executado ao inv�s do m�todo alvo
    /// </summary>
    /// <param name="instance">Inst�ncia real que cont�m o m�todo alvo</param>
    /// <param name="methodInfo">Informa��es sobre a estrutura do m�todo</param>
    /// <param name="args">Par�metros passados para o m�todo</param>
    /// <typeparam name="T">Tipo da inst�ncia real contida no wrapper</typeparam>
    /// <returns>Valor a ser retornado em substitui��o ao retorno do m�todo original</returns>
    public delegate object InsteadOfDelegate<in T>(T instance, MethodInfo methodInfo, object[] args);
}