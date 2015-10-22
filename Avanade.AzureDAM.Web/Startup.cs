using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Avanade.AzureDAM.Web.Startup))]
namespace Avanade.AzureDAM.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
