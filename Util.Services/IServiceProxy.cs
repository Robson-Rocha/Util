using System;

namespace RobsonROX.Util.Services
{
    /// <summary>
    /// Representa novo proxy de serviço WCF
    /// </summary>
    /// <typeparam name="T">Interface de contrato de serviço cujo proxy deverá ser criado</typeparam>
    public interface IServiceProxy<out T> : IDisposable
    {
        /// <summary>
        /// Proxy para o serviço
        /// </summary>
        T Proxy { get; }
    }
}