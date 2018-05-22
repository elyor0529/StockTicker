using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.Common.Memberships
{
    public class UserStore : UserStore<User, Role, long, UserLogin, UserRole, UserClaim>
    {
        public UserStore(DbContext db) : base(db)
        {

        }
    }
}
