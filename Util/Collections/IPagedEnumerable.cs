using System.Collections.Generic;

namespace RobsonROX.Util.Collections
{
    /// <summary>
    /// Interface genérica que adiciona recursos de paginação em coleções enumeráveis
    /// </summary>
    /// <typeparam name="T">Tipo do item enumerável que será contido e adicionado capacidades de enumeração</typeparam>
    public interface IPagedEnumerable<out T> : IEnumerable<IEnumerable<T>>
    {
        /// <summary>
        /// Obtém a coleção original
        /// </summary>
        IEnumerable<T> Source { get; }

        /// <summary>
        /// Obtém o tamanho das páginas
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Obtém a quantidade de páginas
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Obtém uma sub-lista com os itens da página solicitada
        /// </summary>
        /// <param name="page">Número da página cujos itens devem ser retornados</param>
        IEnumerable<T> this[int page] { get; }
    }
}