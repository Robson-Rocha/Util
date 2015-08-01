using System.Web;
using System.Web.Routing;

namespace RobsonROX.Util.MVC.RouteDebug
{
    public class DebugRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext) => new DebugHttpHandler(requestContext);
    }
}