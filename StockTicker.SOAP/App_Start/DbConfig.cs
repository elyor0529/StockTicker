using System;
using System.Configuration;
using System.Data.Entity;
using StockTicker.Lib.DAL;

namespace StockTicker.Soap
{
    public static class DbConfig
    {

        public static void Migrate()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            var isEnabledAutoMigration = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDatabaseAutoMigration"]);

            if (isEnabledAutoMigration)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Migrations.Configuration>());
            }
        }
    }
}