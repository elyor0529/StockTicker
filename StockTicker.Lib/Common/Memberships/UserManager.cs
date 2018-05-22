using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using StockTicker.Lib.DAL;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.Common.Memberships
{
    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public class UserManager : UserManager<User, long>
    {
        public UserManager(UserStore<User, Role, long, UserLogin, UserRole, UserClaim> store)
            : base(store)
        {
        }

        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager(new UserStore<User, Role, long, UserLogin, UserRole, UserClaim>(context.Get<AppDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.            
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, long>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }
            return manager;
        }
         
        public virtual async Task<IdentityResult> AddUserToRolesAsync(long userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<User, long>)Store;
            var user = await FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user);

            // Add user to each role using UserRoleStore
            foreach (var role in roles.Where(w => !userRoles.Contains(w)))
            {
                await userRoleStore.AddToRoleAsync(user, role);
            }

            // Call update once when all roles are added
            return await UpdateAsync(user);
        }

        public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(long userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<User, long>)Store;
            var user = await FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user);

            // Remove user to each role using UserRoleStore
            foreach (var role in roles.Where(w => !userRoles.Contains(w)))
            {
                await userRoleStore.RemoveFromRoleAsync(user, role);
            }

            // Call update once when all roles are removed
            return await UpdateAsync(user);
        }
    }
}