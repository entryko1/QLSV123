using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAnQLSV.Startup))]
namespace DoAnQLSV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
