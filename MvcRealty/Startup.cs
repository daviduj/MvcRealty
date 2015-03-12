using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcRealty.Startup))]
namespace MvcRealty
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
