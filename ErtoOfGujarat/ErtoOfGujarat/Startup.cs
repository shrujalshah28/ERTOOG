using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ErtoOfGujarat.Startup))]
namespace ErtoOfGujarat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
