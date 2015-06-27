namespace RobsonROX.Util.FluentProxy
{
    /// <summary>
    /// Define os métodos para configurações dos métodos da classe proxy
    /// </summary>
    /// <typeparam name="T">Tipo contido pela classe proxy</typeparam>
    public interface IFluentProxyExecuters<out T>
    {
        /// <summary>
        /// Remove quaisquer configurações para o método atual
        /// </summary>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> Clear();

        /// <summary>
        /// Define o método a ser executado antes do método atual
        /// </summary>
        /// <param name="beforeAction">Método a ser executado antes do método atual</param>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> ExecuteBefore(BeforeDelegate<T> beforeAction);

        /// <summary>
        /// Define o método a ser executado depois do método atual
        /// </summary>
        /// <param name="insteadOfAction">Método a ser executado depois do método atual</param>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> ExecuteInstead(InsteadOfDelegate<T> insteadOfAction);

        /// <summary>
        /// Define o método a ser executado ao invés do método atual
        /// </summary>
        /// <param name="afterAction">Método a ser executado ao invés do método atual</param>
        /// <returns>Esta instância</returns>
        IFluentProxyExecuters<T> ExecuteAfter(AfterDelegate<T> afterAction);
    }
}