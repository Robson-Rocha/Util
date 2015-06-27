using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RobsonROX.Util.Collections
{
    /// <summary>
    /// Classe genérica que adiciona recursos de paginação em coleções enumeráveis
    /// </summary>
    /// <typeparam name="T">Tipo do item enumerável que será contido e adicionado capacidades de enumeração</typeparam>
    public class PagedEnumerable<T> : IPagedEnumerable<T>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="source">Coleção enumerável à ser paginada</param>
        /// <param name="pageSize">Tamanho da página</param>
        public PagedEnumerable(IEnumerable<T> source, int pageSize)
        {
            Source = source;
            PageSize = pageSize;
            _pageCount = new Lazy<int>(() => Source.Count());
        }

        /// <summary>
        /// Obtém a coleção original
        /// </summary>
        public IEnumerable<T> Source { get; }

        /// <summary>
        /// Obtém o tamanho das páginas
        /// </summary>
        public int PageSize { get; }

        private readonly Lazy<int> _pageCount; 

        /// <summary>
        /// Obtém a quantidade de páginas
        /// </summary>
        /// <remarks>Cuidado: ao utilizar o PageCount, toda a coleção será enumerada.</remarks>
        public int PageCount => _pageCount.Value / PageSize + _pageCount.Value % PageSize != 0 ? 1 : 0;

        /// <summary>
        /// Obtém uma sub-lista com os itens da página solicitada
        /// </summary>
        /// <param name="page">Número da página cujos itens devem ser retornados. A primeira página obrigatóriamente é zero.</param>
        /// <returns>Retorna os registros da página solicitada.</returns>
        public IEnumerable<T> this[int page] => this[page, false];

        private IEnumerable<T> this[int page, bool returnEmptyAfterLastPageEnd]
        {
            get
            {
                if (page < 0 || (_pageCount.IsValueCreated && page > PageCount))
                    throw new ArgumentOutOfRangeException(nameof(page), page, "A página solicitada não existe.");
                IEnumerable<T> result = Source.Skip(page * PageSize).Take(PageSize).ToArray();
                if(!returnEmptyAfterLastPageEnd && !result.Any())
                    throw new ArgumentOutOfRangeException(nameof(page), page, "A página solicitada não existe.");
                return result;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            int currentPage = 0;
            IEnumerable<T> page;
            while ((page = this[currentPage++, true]).Any())
            {
                yield return page;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}