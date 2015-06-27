using System;
using System.Collections.Generic;
using System.Reflection;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para a classe Type
    /// </summary>
    public static class TypeExtensions
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Dictionary<Type, bool?> _hasDefaultConstructor = new Dictionary<Type, bool?>();

        /// <summary>
        /// Determina se o tipo possui um construtor padrão
        /// </summary>
        public static bool HasDefaultConstructor(this Type type)
        {
            // ReSharper disable once PossibleInvalidOperationException
            return _hasDefaultConstructor.GetOrAdd(type, () => type.IsValueType || type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) != null).Value;
        }

        /// <summary>
        /// Obtém uma instância do tipo extendido
        /// </summary>
        /// <param name="type">Tipo extendido</param>
        /// <param name="args">Argumentos de construtor</param>
        /// <returns>Instância obtida</returns>
        public static object GetInstance(this Type type, params object[] args)
        {
            if ((args == null || args.Length == 0) && !type.HasDefaultConstructor())
                throw new MissingMethodException($"O tipo {type.Name} não possui construtor padrão.");
            return Activator.CreateInstance(type, args);
        }


        /// <summary>
        /// Obtém uma instância do tipo extendido
        /// </summary>
        /// <param name="type">Tipo extendido</param>
        /// <param name="args">Argumentos de construtor</param>
        /// <typeparam name="TInstance">Tipo para o qual a instância deverá ser convertido</typeparam>
        /// <returns>Instância obtida, convertida para o Tipo informado</returns>
        public static TInstance GetInstance<TInstance>(this Type type, params object[] args)
        {
            return (TInstance)GetInstance(type, args);
        }

        /// <summary>
        /// Obtém o valor padrão do tipo extendido
        /// </summary>
        /// <param name="type">Tipo extendido</param>
        /// <returns>Valor padrão do tipo extendido</returns>
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? type.GetInstance() : null;
        }
    }
}
