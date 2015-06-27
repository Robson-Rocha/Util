using System.Reflection;

namespace RobsonROX.Util.FluentProxy
{
    /// <summary>
    /// Define um método a ser executado ao invés do método alvo
    /// </summary>
    /// <param name="instance">Instância real que contém o método alvo</param>
    /// <param name="methodInfo">Informações sobre a estrutura do método</param>
    /// <param name="args">Parâmetros passados para o método</param>
    /// <typeparam name="T">Tipo da instância real contida no wrapper</typeparam>
    /// <returns>Valor a ser retornado em substituição ao retorno do método original</returns>
    public delegate object InsteadOfDelegate<in T>(T instance, MethodInfo methodInfo, object[] args);
}