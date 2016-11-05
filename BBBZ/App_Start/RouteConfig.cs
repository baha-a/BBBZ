using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BBBZ
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Localization route
            routes.MapRoute(
                name : "DefaultLocalized",
                url : "{lang}/{controller}/{action}/{id}",
                defaults : new { controller = "Home", action = "Index", id = UrlParameter.Optional, lang = "en" },
                constraints : new { lang = "[a-z]{2}" }
                );

            routes.MapRoute(
                name : "Default",
                url : "{controller}/{action}/{id}",
                defaults : new { controller = "Home", action = "Index", id = UrlParameter.Optional ,lang = "en" }
            );
        }
    }
}
