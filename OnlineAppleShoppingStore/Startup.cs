using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineAppleShoppingStore.Startup))]
namespace OnlineAppleShoppingStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
