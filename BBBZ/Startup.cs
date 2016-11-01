using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BBBZ.Startup))]
namespace BBBZ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
