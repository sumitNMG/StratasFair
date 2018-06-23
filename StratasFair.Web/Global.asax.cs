using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StratasFair.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            RouteCollection routes = RouteTable.Routes;
            using (routes.GetWriteLock())
            {
                routes.Clear();
                 //repopulate route table here
                AreaRegistration.RegisterAllAreas();
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                DynamicRouteConfig.RegisterRoutes(RouteTable.Routes);
            }
            
        }
    }
}
