using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RobsonROX.Util.Extensions;

namespace RobsonROX.Util.Reflection
{
    /// <summary>
    /// Contém métodos utilitários de Reflection
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Executa um método genérico
        /// </summary>
        /// <param name="sourceType">Tipo onde reside o método a ser executado</param>
        /// <param name="argumentTypes">Tipos dos parâmetros de tipo do método</param>
        /// <param name="instance">Instância de classe do tipo de origem para a execução do método. Se o método for estático, deve ser passado null.</param>
        /// <param name="methodName">Nome do método a ser executado.</param>
        /// <param name="flags">Flags para filtrar os métodos a serem buscados.</param>
        /// <param name="parameters">Parâmetros a serem passados para o método.</param>
        /// <returns>Valor retornado pelo método, e null caso seja void.</returns>
        /// <exception cref="ArgumentNullException">Os argumentos sourceType, argumentTypes e methodName são obrigatórios</exception>
        /// <exception cref="ArgumentException">Caso o método fornecido não seja encontrado, ou ele seja encontrado e os parâmetros fornecidos não correspondam</exception>
        public static object ExecuteGenericMethod(Type sourceType, Type[] argumentTypes, object instance, string methodName, BindingFlags? flags = null, object[] parameters = null)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (argumentTypes == null) throw new ArgumentNullException(nameof(argumentTypes));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            MethodInfo[] methods = (flags != null ? sourceType.GetMethods(flags.Value) : sourceType.GetMethods()).Where(m => m.Name == methodName && m.IsGenericMethod).ToArray();
            if (methods.Length == 0) 
                throw new ArgumentException($"Método {methodName} não encontrado", nameof(methodName));

            foreach (var method in methods)
            {
                var methodParameters = method.GetParameters();
                if ((parameters == null && methodParameters.Length == 0) || (parameters != null && methodParameters.Length == parameters.Length))
                {
                    if (parameters != null)
                    {
                        for(int i = 0, l = parameters.Length; i < l; i++)
                        {
                            if (parameters[i].GetType() != methodParameters[i].ParameterType)
                                goto NextMethod;
                        }
                    }
                    return method.MakeGenericMethod(argumentTypes).Invoke(instance, parameters);
                }
            NextMethod:
                ;
            }

            throw new ArgumentException(
                $"{methods.Length} assinaturas do método {methodName} foram encontradas, mas nenhuma coincidiu com a quantidade e ordem de tipos da lista de parâmetros fornecida.", nameof(methodName));
        }

        /// <summary>
        /// Retorna os valores de propriedades estáticas do mesmo tipo da classe que as contém
        /// </summary>
        /// <typeparam name="TEntity">Tipo onde a busca deve ser realizada</typeparam>
        /// <returns>Lista de valores obtidos</returns>
        public static IEnumerable<KeyValuePair<string, TEntity>> GetStaticFieldsWithSameType<TEntity>()
        {
            return TypeCache<TEntity>.Type
                                     .GetFields(BindingFlags.Static | BindingFlags.Public)
                                     .Where(p => p.FieldType == TypeCache<TEntity>.Type)
                                     .Select(p => new KeyValuePair<string, TEntity>(p.Name, (TEntity)p.GetValue(null)));
        }

        private static readonly IDictionary<string, IEnumerable<Type>> AssignableTypes = new Dictionary<string, IEnumerable<Type>>();
        /// <summary>
        /// Obtém todos os tipos que herdam o tipo especificado em T
        /// </summary>
        /// <param name="fqn">Opcional. Se especificado, só busca assemblies com o FQN especificado, ou que contenham em seu FQN a string especificada. Se omitido ou null, obtém todos os assemblies.</param>
        /// <typeparam name="T">Tipo cujo qual se deve buscar implementadores</typeparam>
        /// <returns>IEnumerable{Type} contendo todos os tipos que herdam de T</returns>
        public static IEnumerable<Type> GetAssignableTypesFrom<T>(string fqn = null)
        {
            var type = TypeCache<T>.Type;
            return AssignableTypes.GetOrAdd(type.FullName, () => AppDomain.CurrentDomain
                                                                           .GetAssemblies()
                                                                           .Where(a => fqn == null || a.FullName.Contains(fqn))
                                                                           .SelectMany(s => s.GetTypes())
                                                                           .Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract));
        }
    }
}
