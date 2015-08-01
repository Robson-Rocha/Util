using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RobsonROX.Util.MVC.Routes
{
    public class DelegateRouteConstraint : IRouteConstraint
    {
        private readonly string[] _routeSegments;
        private readonly Func<object, bool> _constraintValidator;

        /// <summary>
        /// Atribui uma função de validação para um segmento de rota específico a ser procurado na rota
        /// </summary>
        /// <param name="constraintValidator">Função para análise do segmento de rota</param>
        /// <param name="routeSegments">Segmentos de rota a serem analisados</param>
        /// <exception cref="ArgumentNullException">O segmento de rota e a função de validação do segmento não pode ser nulos</exception>
        public DelegateRouteConstraint(Func<object, bool> constraintValidator, params string[] routeSegments)
        {
            if (routeSegments == null) throw new ArgumentNullException(nameof(routeSegments));
            if (constraintValidator == null) throw new ArgumentNullException(nameof(constraintValidator));

            _routeSegments = routeSegments;
            _constraintValidator = constraintValidator;
        }

        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param><param name="route">The object that this constraint belongs to.</param><param name="parameterName">The name of the parameter that is being checked.</param><param name="values">An object that contains the parameters for the URL.</param><param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _routeSegments.All(s => values.Keys.Contains(s)) &&
                   _routeSegments.All(s => _constraintValidator(values[s]));
        }
    }
}
