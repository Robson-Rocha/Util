using System;

namespace RobsonROX.Util.Required
{
    /// <summary>
    /// Exceção lançada quando uma variável de um tipo marcado como requerido não foi inicializada corretamente
    /// </summary>
    [Serializable]
    public class RequiredNotInitializedException<T> : Exception
        where T : class
    {
        /// <summary>
        /// Cria uma nova instância da exceção, indicando o nome do tipo requerido não informado
        /// </summary>
        public RequiredNotInitializedException() : base(message: $"Uma variável do tipo {typeof(T).FullName}, marcado como requerido, não foi inicializada corretamente.")
        {
        }
    }
}