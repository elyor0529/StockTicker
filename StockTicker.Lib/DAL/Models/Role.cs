using Microsoft.AspNet.Identity.EntityFramework;
using StockTicker.Lib.Common.Memberships;

namespace StockTicker.Lib.DAL.Models
{
    public class Role : IdentityRole<long, UserRole>
    {
        public MemberRole Code { get; set; }
    }
}
