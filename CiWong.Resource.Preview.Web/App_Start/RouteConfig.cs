using System.Web.Mvc;
using System.Web.Routing;

namespace CiWong.Resource.Preview.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("flash/player.swf");

            routes.MapRoute(
                name: "error",
                url: "error",
                defaults: new { controller = "Home", action = "Error" },
                namespaces: new string[] { "CiWong.Resource.Preview.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CiWong.Resource.Preview.Web.Controllers" }
            );
        }
    }
}