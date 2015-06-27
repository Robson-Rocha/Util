using System.Collections.ObjectModel;
using System.Linq;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para a classe <see cref="ObservableCollection{T}"/>
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Remove todos os itens da coleção, um a um, de forma a disparar as notificações de observação
        /// </summary>
        /// <param name="collection">Coleção observável extendida</param>
        /// <typeparam name="T">Tipo contido pela coleção</typeparam>
        public static void ObservableClear<T>(this ObservableCollection<T> collection)
        {
            var removed = collection.ToList();
            foreach (var item in removed)
            {
                collection.Remove(item);
            }
        }
    }
}
