using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Medcenter.Service.MVC5.Startup))]
namespace Medcenter.Service.MVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
