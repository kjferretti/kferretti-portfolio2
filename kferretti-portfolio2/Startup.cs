using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(kferretti_portfolio2.Startup))]
namespace kferretti_portfolio2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
