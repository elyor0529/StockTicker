using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.DynamicFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StockTicker.Lib.Common.Exceptions;
using StockTicker.Lib.Common.Utils;
using StockTicker.Lib.DAL.EF;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, long, UserLogin, UserRole, UserClaim>, IDbContext
    {
        public AppDbContext()
            : base("DefaultConnection")
        {
            Database.CommandTimeout = 3600;
            Database.Log += delegate (string s) { Logging.Info(s); };
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Stock> Stocks { get; set; }

        public override int SaveChanges()
        {
            try
            {
                var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => x.Entity is BaseEntity
                                &&
                                (x.State == EntityState.Added || x.State == EntityState.Modified ||
                                 x.State == EntityState.Deleted));

                foreach (var entry in modifiedEntries)
                {
                    var entity = (BaseEntity)entry.Entity;
                    var identityName = Thread.CurrentPrincipal.Identity.GetUserName();
                    var now = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }

                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                Logging.Error(exceptionMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new StockException(exceptionMessage, ex);
            }
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);

            dbEntityEntry.State = EntityState.Added;
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);

            dbEntityEntry.State = EntityState.Modified;
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);

            dbEntityEntry.State = EntityState.Deleted;
        }

        private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntityEntry = Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
                Set<TEntity>().Attach(entity);

            return dbEntityEntry;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<Role>().ToTable("Roles");

            // Multiple navigation property filter
            modelBuilder.Filter("IsDeleted", (BaseEntity f) => f.IsDeleted, false);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => SaveChanges(), cancellationToken);
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}