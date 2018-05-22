using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.Common.Memberships
{
    public class RoleStore : RoleStore<Role, long, UserRole>
    {
        public RoleStore(DbContext db) : base(db)
        {

        }
    }
}
