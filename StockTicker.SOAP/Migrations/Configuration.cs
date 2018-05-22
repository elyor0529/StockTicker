using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using StockTicker.Lib.Common.Const;
using StockTicker.Lib.Common.Extensions;
using StockTicker.Lib.Common.Memberships;
using StockTicker.Lib.Common.Utils;
using StockTicker.Lib.DAL;
using StockTicker.Lib.DAL.EF;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Soap.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            //config
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            CommandTimeout = 3600;
            ContextKey = "ASMX.Lib.DAL.AppDbContext";

            //interception
            DbInterception.Add(new EFInterceptor());
        }

        protected override void Seed(AppDbContext context)
        {
            //role
            var roleManager = new RoleManager(new RoleStore(context));
            var role = roleManager.FindById(1);
            if (role == null)
            {
                roleManager.Create(new Role
                {
                    Id = 1,
                    Name = MemberRole.Developer.GetName(),
                    Code = MemberRole.Developer
                });
            }

            //users
            var userManager = new UserManager(new UserStore(context));
            var user = userManager.FindById(1);
            if (user == null)
            {
                user = new User
                {
                    Id = 1,
                    FullName = "Elyor Latipov",
                    Email = "elyor@outlook.com",
                    UserName = "elyor@outlook.com",
                    EmailConfirmed = true
                };
                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, RoleKeys.Developer);
            }

            //stocks
            var stocks = new Stock[]
            {

                new Stock {UserId = 1, Symbol = "MSFT", Name = "Microsoft", Price = RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "DELL", Name = "Dell Computers", Price =RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "HWP", Name = "Hewlett Packard", Price = RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "YHOO", Name = "Yahoo!", Price = RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "GE", Name = "General Electric", Price = RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "IBM", Name = "International Business Machine", Price = RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "GM", Name = "General Motors", Price = RandomHelper.GetNumber()},
                new Stock {UserId = 1, Symbol = "F", Name = "Ford Motor Company", Price =RandomHelper.GetNumber()}
            };
            context.Stocks.AddOrUpdate(p => p.Symbol, stocks);

            context.SaveChanges();

        }
    }
}
