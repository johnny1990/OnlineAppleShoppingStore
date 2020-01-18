using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineAppleShoppingStore.Web.Startup))]
namespace OnlineAppleShoppingStore.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
