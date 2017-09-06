using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KYHBPA_TeamA.Startup))]
namespace KYHBPA_TeamA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
