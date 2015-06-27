using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para enumeradores
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Obtém a instância de um atributo do tipo especificado que tenha sido aplicado ao valor fornecido da enumeração
        /// </summary>
        /// <param name="enumValue">Valor da enumeração cujo atributo do tipo especificado deve ser procurado</param>
        /// <typeparam name="T">Tipo do atributo a ser </typeparam>
        /// <returns>Primeira instância do atributo encontrado. Caso nenhum atributo com o tipo especificado tenha sido encontrado, retorna null.</returns>
        public static T GetAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            return (T) memberInfo?.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        /// <summary>
        /// Obtém o a descrição atribuída através do atributo <see cref="System.ComponentModel.DescriptionAttribute"/> no valor da enumeração fornecido
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum enumValue)
        {
            var attr = enumValue.GetAttribute<DescriptionAttribute>();
            return (attr != null ? attr.Description : enumValue.ToString());
        }

        /// <summary>
        /// Retorna os valores da enumeração, como um <see cref="System.Collections.Generic.IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">Tipo da enumneração</typeparam>
        /// <returns>Enumerável contendo todos os valores da enumeração</returns>
        public static IEnumerable<T> AsEnumerable<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
