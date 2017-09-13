using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Adviser.Startup))]
namespace Adviser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
