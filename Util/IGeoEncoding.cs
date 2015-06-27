using System;
using System.Collections.Generic;
using System.Device.Location;

// ReSharper disable once CheckNamespace
namespace KNPX.DeliveryServices.Framework.GeoLocation
{
    /// <summary>
    /// Provê meios para interagir com provedores de GeoEncodificação
    /// </summary>
    public interface IGeoEncoding
    {
        /// <summary>
        /// GeoEncodifica um par de coordenadas em um endereço
        /// </summary>
        /// <param name="coords">Coordenadas</param>
        /// <exception cref="ArgumentNullException">A coordenada é obrigatória</exception>
        /// <returns>Endereço</returns>
        List<string> GetAddresses(GeoCoordinate coords);

        /// <summary>
        /// GeoEncodifica um endereço em coordenadas
        /// </summary>
        /// <param name="address">Endereço</param>
        /// <exception cref="ArgumentNullException">O endereço é obrigatório.</exception>
        /// <returns>Coordenadas</returns>
        GeoCoordinate GetCordinates(string address);

        /// <summary>
        /// Calcula a distância entre duas coordenadas
        /// </summary>
        /// <param name="fromCoordinate">Origem</param>
        /// <param name="toCoordinate">Destino</param>
        /// <returns>Distância</returns>
        /// <exception cref="ArgumentNullException">Ambas as coordenadas são obrigatórias</exception>
        double GetDistance(GeoCoordinate fromCoordinate, GeoCoordinate toCoordinate);

        /// <summary>
        /// Obtém a distância entre dois endereços
        /// </summary>
        /// <param name="address1">Origem</param>
        /// <param name="address2">Destino</param>
        /// <returns>Distância</returns>
        double GetDistance(string address1, string address2);
    }
}
