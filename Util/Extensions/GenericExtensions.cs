using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using RobsonROX.Util.Reflection;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Extensões genéricas, aplicáveis a qualquer tipo
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Converte o tipo da struct fornecida em Nulável
        /// </summary>
        /// <param name="source">struct a ser convertida em nulável</param>
        /// <typeparam name="T">Tipo da struct a ser convertida em nulável</typeparam>
        /// <returns>Nullable{T}</returns>
        public static T? AsNullable<T>(this T source)
            where T : struct
        {
            return source;
        }

        /// <summary>
        /// Verifica se um dos itens contidos na lista de parâmetros é igual ao item comparado
        /// </summary>
        /// <param name="source">Item comparado</param>
        /// <param name="list">Lista para comparação</param>
        /// <typeparam name="T">Tipo a ser comparado</typeparam>
        /// <returns>true se um dos itens a serem comparados é igual ao item comparado</returns>
        public static bool In<T>(this T source, params T[] list)
        {
            return list.Any(l => l.Equals(source));
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (TypeCache<T>.Type.IsValueType)
                return source;

            if (!TypeCache<T>.Type.IsSerializable)
                throw new ArgumentException($"{TypeCache<T>.Type.Name} não é serializável", nameof(source));
    
            if (ReferenceEquals(source, null))
                return default(T);

            var serializer = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                serializer.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
