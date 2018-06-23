using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StratasFair.Web.Startup))]
namespace StratasFair.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        { 
            ConfigureAuth(app);
        }
    }
}
