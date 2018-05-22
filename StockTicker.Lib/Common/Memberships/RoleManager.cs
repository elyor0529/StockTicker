using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using StockTicker.Lib.DAL;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.Common.Memberships
{
    public class RoleManager : RoleManager<Role, long>
    {
        public RoleManager(RoleStore<Role, long, UserRole> roleStore)
        : base(roleStore)
        { }

        public static RoleManager Create(
            IdentityFactoryOptions<RoleManager> options,
            IOwinContext context)
        {
            var manager = new RoleManager(new RoleStore<Role, long, UserRole>(context.Get<AppDbContext>()));

            return manager;
        }
    }
}
