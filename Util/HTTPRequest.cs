using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;

namespace RobsonROX.Util
{
    /// <summary>
    /// Permite realizar requisições HTTP de baixo nível de maneira simplificada
    /// </summary>
    public static class HttpRequest
    {
        /// <summary>
        /// Representa os tipos de encodificação de conteúdo suportados
        /// </summary>
        public static class ContentEncodings
        {
            /// <summary>
            /// UTF-8
            /// </summary>
            public const string Utf8 = "UTF-8";

            /// <summary>
            /// ISO-8859-1
            /// </summary>
            public const string Iso88951 = "ISO-8859-1";
        }

        /// <summary>
        /// Representa os métodos HTTP suportados
        /// </summary>
        public static class HttpMethods
        {
            /// <summary>
            /// GET
            /// </summary>
            public const string Get = "GET";

            /// <summary>
            /// POST
            /// </summary>
            public const string Post = "POST";
        }

        /// <summary>
        /// Representa os tipos de encodificação de formulário suportados
        /// </summary>
        public static class ContentTypes
        {
            /// <summary>
            /// application/x-www-form-urlencoded -- Padrão do HTML
            /// </summary>
            public const string WwwFormUrlEncoded = "application/x-www-form-urlencoded";

            /// <summary>
            /// multipart/form-data -- Usado em uploads
            /// </summary>
            public const string MultipartFormData = "multipart/form-data";
        }


        /// <summary>
        /// Efetua uma requisição HTTP e retorna o resultado da requisição decodificado como texto
        /// </summary>
        /// <param name="url">Representa a URL a ser requisitada</param>
        /// <param name="bodyItems">Lista de pares nome/valor com os campos do formulário a serem enviados</param>
        /// <param name="httpMethod">Método HTTP utilizado na requisição</param>
        /// <param name="contentEncoding">Tipo de encodificação dos textos da resposta</param>
        /// <param name="networkCredential"></param>
        /// <returns>String contendo o texto decodificado</returns>
        /// <exception cref="NotSupportedException">ContentTypes.MultipartFormData ainda não é suportado</exception>
        public static string Request(Uri url, 
                                     NameValueCollection bodyItems = null,
                                     string httpMethod = HttpMethods.Get, 
                                     string contentEncoding = ContentEncodings.Iso88951,
                                     NetworkCredential networkCredential = null)
        {
            using (var client = new WebClient())
            {
                if(networkCredential != null) client.Proxy.Credentials = networkCredential;
                try
                {
                    return httpMethod == HttpMethods.Get ?
                        client.DownloadString(url) :
                        Encoding.GetEncoding(contentEncoding).GetString(client.UploadValues(url, httpMethod, bodyItems ?? new NameValueCollection()));
                }
                catch (WebException webException)
                {
                    if (webException.Message.Contains("407") && networkCredential == null && ConfigurationManager.AppSettings["ProxyUserName"] != null && ConfigurationManager.AppSettings["ProxyPassword"] != null)
                    {
                        networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"], ConfigurationManager.AppSettings["ProxyPassword"]);
                        return Request(url, bodyItems, httpMethod, contentEncoding, networkCredential);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Efetua uma requisição HTTP e retorna o resultado da requisição decodificado como texto
        /// </summary>
        /// <param name="url">Representa a URL a ser requisitada</param>
        /// <param name="body">String de consulta no formato "item=valor&amp;item=valor" com os campos do formulário a serem enviados</param>
        /// <param name="httpMethod">Método HTTP utilizado na requisição</param>
        /// <param name="contentEncoding">Tipo de encodificação dos textos da resposta</param>
        /// <returns>String contendo o texto decodificado</returns>
        /// <exception cref="NotSupportedException">ContentTypes.MultipartFormData ainda não é suportado</exception>
        public static string Request(string url, 
                                     string body = null,
                                     string httpMethod = HttpMethods.Get,
                                     string contentEncoding = ContentEncodings.Iso88951)
        {
            var data = new NameValueCollection();
            if (body != null)
            {
                var pairs = body.Split('&');
                foreach (var item in pairs)
                {
                    var pair = item.Split('=');
                    data.Add(pair[0], pair[1]);
                }
            }
            return Request(new Uri(url), data, httpMethod, contentEncoding);
        }
    }
}
