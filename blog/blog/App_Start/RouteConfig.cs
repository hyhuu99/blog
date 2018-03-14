using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Post",
                url: "{controller}/{id}/{slug}",
                defaults: new { controller = "Posts", action = "Details", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Tag",
            //    url: "{controller}/{slug}",
            //    defaults: new { controller = "Tags", action = "Details" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{slug}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, slug = UrlParameter.Optional }
                
            );            
        }
    }
}
