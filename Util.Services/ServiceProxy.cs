using System;
using System.ServiceModel;

namespace RobsonROX.Util.Services
{
    /// <summary>
    /// Cria um novo proxy de serviço WCF
    /// </summary>
    /// <typeparam name="T">Interface de contrato de serviço cujo proxy deverá ser criado</typeparam>
    public class ServiceProxy<T> : IServiceProxy<T>
    {
        private readonly Lazy<T> _proxy;
        private readonly string _endpointName;

        /// <summary>
        /// Proxy para o serviço
        /// </summary>
        public T Proxy => _proxy.Value;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="endpointName">Opcional. Nome do endpoint a ser utilizado. Se omitido, é inferido pelo WCF com base no nome da interface de contrato de serviço.</param>
        public ServiceProxy(string endpointName = null)
        {
            _endpointName = endpointName;
            _proxy = new Lazy<T>(() =>
                _endpointName == null ?
                new ChannelFactory<T>().CreateChannel() :
                new ChannelFactory<T>(_endpointName).CreateChannel());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="managed">If true, frees only managed resources</param>
        protected virtual void Dispose(bool managed)
        {
            if (_proxy.IsValueCreated)
            {
                ((ICommunicationObject)_proxy.Value).Close();
                ((IDisposable)_proxy.Value).Dispose();
            }
            if (managed)
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ServiceProxy()
        {
            Dispose(false);
        }
    }
}
