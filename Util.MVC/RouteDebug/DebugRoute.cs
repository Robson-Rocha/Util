using System.Web.Routing;

namespace RobsonROX.Util.MVC.RouteDebug
{
    public class DebugRoute : Route
    {
        public static DebugRoute Singleton { get; } = new DebugRoute();

        private DebugRoute()
            : base("{*catchall}", new DebugRouteHandler())
        { }
    }
}