using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SPF.OWAES
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //routes.IgnoreRoute("{*url}", new { url = @".*SingPassAssertionConsumerService.ashx"});
            //routes.IgnoreRoute("{*url}", new { url = @".*SingPassSingleLogoutService.ashx"});
            //routes.IgnoreRoute("{*url}", new { url = @".*CorpPassAssertionConsumerService.ashx"});
            //routes.IgnoreRoute("{*url}", new { url = @".*CorpPassSingleLogoutService.ashx" });

            routes.Ignore("{*url}", new { url = @".*SingPassAssertionConsumerService.ashx" });
            routes.Ignore("{*url}", new { url = @".*SingPassSingleLogoutService.ashx" });
            routes.Ignore("{*url}", new { url = @".*CorpPassAssertionConsumerService.ashx" });
            routes.Ignore("{*url}", new { url = @".*CorpPassSingleLogoutService.ashx" });
            routes.IgnoreRoute("elmah.axd");

            routes.MapRoute(
               name: "Help",
               url: "Help",
               defaults: new { controller = "Home", action = "Help", id = UrlParameter.Optional }
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
            );


        }

        

    }
}
