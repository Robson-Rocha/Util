using System.Reflection;

namespace RobsonROX.Util.FluentProxy
{
    /// <summary>
    /// Define um método a ser executado antes do método alvo
    /// </summary>
    /// <param name="instance">Instância real que contém o método alvo</param>
    /// <param name="methodInfo">Informações sobre a estrutura do método</param>
    /// <param name="args">Parâmetros passados para o método</param>
    /// <param name="returnedValue">Valor retornado pelo método alvo</param>
    /// <typeparam name="T">Tipo da instância real contida no wrapper</typeparam>
    public delegate void AfterDelegate<in T>(T instance, MethodInfo methodInfo, object[] args, object returnedValue);
}