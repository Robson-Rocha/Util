using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RobsonROX.Util.Collections;

namespace RobsonROX.Util.Extensions
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable PossibleMultipleEnumeration

    /// <summary>
    /// Métodos de extensão para IEnumerable
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Retorna uma versão paginável da coleção
        /// </summary>
        /// <param name="source">Coleção a ser paginável</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <typeparam name="T">Tipo contido pela coleção</typeparam>
        /// <returns>Uma instância de <see cref="PagedEnumerable{T}"/> contendo a coleção fornecida</returns>
        [DebuggerStepThrough]
        public static IPagedEnumerable<T> AsPagedEnumerable<T>(this IEnumerable<T> source, int pageSize)
        {
            return new PagedEnumerable<T>(source, pageSize);
        }

        /// <summary>
        /// Executa o delegate especificado para todos os itens da coleção fornecida
        /// </summary>
        /// <param name="source">Coleção a ser iterada</param>
        /// <param name="action">Delegate representando a ação a ser executada</param>
        /// <typeparam name="T">Tipo dos itens da coleção fornecida</typeparam>
        /// <returns>Coleção fornecida como source</returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
            return source;
        }

        /// <summary>
        /// Executa o delegate especificado para todos os itens da coleção fornecida, permitindo interromper a iteração
        /// </summary>
        /// <param name="source">Coleção a ser iterada</param>
        /// <param name="breakableFunc">Delegate representando a ação a ser executada. Caso o delegate retorne false, interrompe a iteração</param>
        /// <typeparam name="T">Tipo dos itens da coleção fornecida</typeparam>
        /// <returns>Coleção fornecida como source</returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Func<T, bool> breakableFunc)
        {
            foreach (var item in source)
                if(!breakableFunc(item)) break;
            return source;
        }

        /// <summary>
        /// Filtra uma lista com base no critério apresentado pelo selector
        /// </summary>
        /// <param name="source">Lista a ser filtrada</param>
        /// <param name="keySelector">Função que indica qual o critério de filtragem</param>
        /// <typeparam name="TSource">Tipo de lista de origem</typeparam>
        /// <typeparam name="TKey">Tipo do valor retornado pela função de critério</typeparam>
        /// <returns>Lista contendo apenas os itens únicos de acordo com os critérios retornados pela função para cada elemento da lista de origem</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var set = new HashSet<TKey>();
            return source.Where(s => set.Add(keySelector(s)));
        }
    }
}
