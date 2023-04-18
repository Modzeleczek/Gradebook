using System.Web.Mvc;
using System.Web.Routing;

namespace Gradebook
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                // /Home/Index immediately redirects to announcements.
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "Gradebook.Controllers" }
            );
        }
    }
}
