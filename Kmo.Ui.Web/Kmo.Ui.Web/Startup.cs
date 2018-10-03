using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kmo.Startup))]
namespace Kmo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}