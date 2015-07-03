using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bware.Startup))]
namespace Bware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
