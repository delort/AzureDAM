using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Avanade.AzureDAM.API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "GetAsset",
              url: "asset/{channel}/{*id}",
              defaults: new { controller = "Assets", action = "Get", channel = "", id = "" }
              );

            routes.MapRoute(
                name: "DefaultAssets",
                url: "{controller}/{action}/{params}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

          
        }
    }
}
