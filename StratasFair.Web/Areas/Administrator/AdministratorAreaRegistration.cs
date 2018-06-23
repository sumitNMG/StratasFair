using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator
{
    public class AdministratorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administrator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administrator_default",
                "securehost/rootlogin/{controller}/{action}/{id}",
                new { action = "Index", Controller = "LogOn", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Areas.Administrator.Controllers" }
            );
        }
    }
}