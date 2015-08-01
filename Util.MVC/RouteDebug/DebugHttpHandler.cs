using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RobsonROX.Util.MVC.RouteDebug
{
    public class DebugHttpHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public DebugHttpHandler(RequestContext requestContext)
        {
            RequestContext = requestContext;
        }

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            string generatedUrlInfo = string.Empty;
            if (context.Request.QueryString.Count > 0)
            {
                var rvalues = new RouteValueDictionary();
                foreach (string key in context.Request.QueryString.Keys)
                {
                    rvalues.Add(key, context.Request.QueryString[key]);
                }

                var vpd = RouteTable.Routes.GetVirtualPath(RequestContext, rvalues);
                if (vpd != null)
                {
                    generatedUrlInfo = "<p><label>Generated URL</label>: ";
                    generatedUrlInfo += "<strong style=\"color: #00a;\">" + vpd.VirtualPath + "</strong>";
                    var vpdRoute = vpd.Route as Route;
                    if (vpdRoute != null)
                    {
                        generatedUrlInfo += " using the route \"" + vpdRoute.Url + "\"</p>";
                    }
                }
            }

            string htmlFormat = @"<html>
<head>
    <title>Route Tester</title>
    <style>
        body, td, th {{font-family: verdana; font-size: small;}}
        .message {{font-size: .9em;}}
        caption {{font-weight: bold;}}
        tr.header {{background-color: #ffc;}}
        label {{font-weight: bold; font-size: 1.1em;}}
        .false {{color: #c00;}}
        .true {{color: #0c0;}}
    </style>
</head>
<body>
<h1>Route Tester</h1>
<div id=""main"">
    <p class=""message"">
        Type in a url in the address bar to see which defined routes match it. 
        A {{*catchall}} route is added to the list of routes automatically in 
        case none of your routes match.
    </p>
    <p class=""message"">
        To generate URLs using routing, supply route values via the query string. example: <code>http://localhost:14230/?id=123</code>
    </p>
    <p><label>Matched Route</label>: {1}</p>
    {5}
    <div style=""float: left;"">
        <table border=""1"" cellpadding=""3"" cellspacing=""0"" width=""300"">
            <caption>Route Data</caption>
            <tr class=""header""><th>Key</th><th>Value</th></tr>
            {0}
        </table>
    </div>
    <div style=""float: left; margin-left: 10px;"">
        <table border=""1"" cellpadding=""3"" cellspacing=""0"" width=""300"">
            <caption>Data Tokens</caption>
            <tr class=""header""><th>Key</th><th>Value</th></tr>
            {4}
        </table>
    </div>
    <hr style=""clear: both;"" />
    <table border=""1"" cellpadding=""3"" cellspacing=""0"">
        <caption>All Routes</caption>
        <tr class=""header"">
            <th>Matches Current Request</th>
            <th>Url</th>
            <th>Defaults</th>
            <th>Constraints</th>
            <th>DataTokens</th>
        </tr>
        {2}
    </table>
    <hr />
    <h3>Current Request Info</h3>
    <p>
        AppRelativeCurrentExecutionFilePath is the portion of the request that Routing acts on.
    </p>
    <p><strong>AppRelativeCurrentExecutionFilePath</strong>: {3}</p>
</div>
</body>
</html>";
            string routeDataRows = string.Empty;

            RouteData routeData = RequestContext.RouteData;
            RouteValueDictionary routeValues = routeData.Values;
            RouteBase matchedRouteBase = routeData.Route;

            string routes = string.Empty;
            using (RouteTable.Routes.GetReadLock())
            {
                foreach (RouteBase routeBase in RouteTable.Routes)
                {
                    bool matchesCurrentRequest = (routeBase.GetRouteData(RequestContext.HttpContext) != null);
                    string matchText = $@"<span class=""{matchesCurrentRequest}"">{matchesCurrentRequest}</span>";
                    string url = "n/a";
                    string defaults = "n/a";
                    string constraints = "n/a";
                    string dataTokens = "n/a";

                    Route route = routeBase as Route;
                    if (route != null)
                    {
                        url = route.Url;
                        defaults = FormatRouteValueDictionary(route.Defaults);
                        constraints = FormatRouteValueDictionary(route.Constraints);
                        dataTokens = FormatRouteValueDictionary(route.DataTokens);
                    }

                    routes += $@"<tr><td>{matchText}</td><td>{url}</td><td>{defaults}</td><td>{constraints}</td><td>{dataTokens}</td></tr>";
                }
            }

            string matchedRouteUrl = "n/a";

            string dataTokensRows = "";

            if (!(matchedRouteBase is DebugRoute))
            {
                routeDataRows = routeValues.Keys.Aggregate(routeDataRows, (current, key) => current + $"\t<tr><td>{key}</td><td>{routeValues[key]}&nbsp;</td></tr>");

                dataTokensRows = routeData.DataTokens.Keys.Aggregate(dataTokensRows, (current, key) => current + $"\t<tr><td>{key}</td><td>{routeData.DataTokens[key]}&nbsp;</td></tr>");

                Route matchedRoute = matchedRouteBase as Route;

                if (matchedRoute != null)
                    matchedRouteUrl = matchedRoute.Url;
            }
            else
            {
                matchedRouteUrl = "<strong class=\"false\">NO MATCH!</strong>";
            }

            context.Response.Write(string.Format(htmlFormat
                , routeDataRows
                , matchedRouteUrl
                , routes
                , context.Request.AppRelativeCurrentExecutionFilePath
                , dataTokensRows
                , generatedUrlInfo));
        }

        private static string FormatRouteValueDictionary(RouteValueDictionary values)
        {
            if (values == null || values.Count == 0)
                return "(null)";

            string display = values.Keys.Aggregate(string.Empty, (current, key) => current + $"{key} = {values[key]}, ");
            if (display.EndsWith(", "))
                display = display.Substring(0, display.Length - 2);
            return display;
        }
    }
}