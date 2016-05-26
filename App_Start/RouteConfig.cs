using System.Web.Mvc;
using System.Web.Routing;

namespace Islam
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();
            //AreaRegistration.RegisterAllAreas();

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{MenuEng}",
            //    defaults: new { controller = "Home", action = "Articles", MenuEng = UrlParameter.Optional },
            //    namespaces: new[] { "Islam.Controllers" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{Id}/{S_Id}",
                defaults: new { controller = "Home", action = "Index", Id = UrlParameter.Optional, S_Id = UrlParameter.Optional}
                //namespaces: new[] { "Islam.Controllers" }
            );
        }
    }
}
