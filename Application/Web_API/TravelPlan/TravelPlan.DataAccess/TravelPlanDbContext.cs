using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TravelPlan.DataAccess
{
    public class TravelPlanDbContext: DbContext
    {
        public TravelPlanDbContext(DbContextOptions<TravelPlanDbContext> options): base(options)
        { }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TravelPlanDbContext>
    {
        public TravelPlanDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../TravelPlan.API/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<TravelPlanDbContext>();

            //NOTE: this code is executed when a migration is being created in this class library.
            //The execution of the traget project (API) has nothing to do with these connection strings.

            //Use this connection string when in production mode (when using the database hosted on Azure).
            var connectionString = configuration.GetConnectionString("ProdConnString");

            //Use this connection string when in development mode (when using local database).
            //var connectionString = configuration.GetConnectionString("DevelopConnString");

            builder.UseSqlServer(connectionString);
            return new TravelPlanDbContext(builder.Options);
        }
    }
}
