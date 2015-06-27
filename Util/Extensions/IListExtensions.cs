using System.Collections.Generic;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para IList
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IListExtensions
    {
        /// <summary>
        /// Permite inserir um novo valor na lista caso ele não exista previamente
        /// </summary>
        /// <param name="source">Lista de onde se pretende adicionar condicionalmente o valor</param>
        /// <param name="value">Valor a ser inserido na lista em caso o mesmo não tenha sido encontrado.</param>
        /// <typeparam name="TValue">Tipo do valor</typeparam>
        public static void AddIfNotExists<TValue>(this IList<TValue> source, TValue value)
        {
            if (!source.Contains(value))
                source.Add(value);
        }
    }
}