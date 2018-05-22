using Microsoft.Owin;
using Owin;
using StockTicker.Soap;

[assembly: OwinStartup(typeof(Startup))]
namespace StockTicker.Soap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            DbConfig.Migrate();
        }
    }
}
