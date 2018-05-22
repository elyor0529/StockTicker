using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StockTicker.Lib.DAL.Models
{
    public class User : IdentityUser<long, UserLogin, UserRole, UserClaim>
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
          
        public virtual ICollection<Stock> Stocks { get; set; }

        public User()
        {
            Stocks = new HashSet<Stock>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, long> manager, string authenticationType = DefaultAuthenticationTypes.ApplicationCookie)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }

    }
}