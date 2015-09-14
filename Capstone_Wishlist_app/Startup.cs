using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Capstone_Wishlist_app.Startup))]
namespace Capstone_Wishlist_app
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
