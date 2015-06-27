using System;
using RobsonROX.Util.Reflection;

namespace RobsonROX.Util
{
    /// <summary>
    /// Encapsula uma única instância de T, especialmente para classes com construtor privado
    /// </summary>
    /// <typeparam name="T">Tipo que representa uma classe a ser encapsulada</typeparam>
    public static class Singleton<T>
        where T : class
    {
        private static volatile T _instance;

        // ReSharper disable once StaticFieldInGenericType
        private static readonly object LockToken = new object();

        /// <summary>
        /// Provê acesso (e se necessário, inicialização) à instância contida
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockToken)
                    {
                        if (_instance == null) _instance = Activator.CreateInstance(TypeCache<T>.Type, true) as T;
                    }
                }

                return _instance;
            }
        }
    }
}