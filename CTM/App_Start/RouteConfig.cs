using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CTM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Localization: http://stackoverflow.com/questions/1560796/set-culture-in-an-asp-net-mvc-app/1561583#1561583
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional }
               
            ).DataTokens = new RouteValueDictionary(new { area = "Home" }); ;
        }
    }
}
