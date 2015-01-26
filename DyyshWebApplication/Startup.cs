using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DyyshWebApplication.Startup))]
namespace DyyshWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
