using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CTM.Startup))]
namespace CTM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
