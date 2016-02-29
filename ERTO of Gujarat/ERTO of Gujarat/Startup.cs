using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERTO_of_Gujarat.Startup))]
namespace ERTO_of_Gujarat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
