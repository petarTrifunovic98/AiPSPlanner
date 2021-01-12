using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.DataAccess
{
    public class TravelPlanDbContext: DbContext
    {
        public TravelPlanDbContext(DbContextOptions<TravelPlanDbContext> options): base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<AccommodationPicture> AccommodationPictures { get; set; }
        public DbSet<Votable> Votables { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<SeaAddOn> SeaAddOns { get; set; }
        public DbSet<WinterAddOn> WinterAddOns { get; set; }
        public DbSet<SpaAddOn> SpaAddOns { get; set; }
        public DbSet<TripType> TripTypes { get; set; }
        public DbSet<SeaType> SeaTypes { get; set; }
        public DbSet<WinterType> WinterTypes { get; set; }
        public DbSet<SpaType> SpaTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>()
                .HasOne(a => a.Location)
                .WithMany(l => l.Accommodations)
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Accommodations)
                .WithOne(a => a.Location)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }   


    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TravelPlanDbContext>
    {
        public TravelPlanDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../TravelPlan/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<TravelPlanDbContext>();

            //NOTE: this code is executed when a migration is being created in this class library.
            //The execution of the traget project (API) has nothing to do with these connection strings.

            //Use this connection string when in production mode (when using the database hosted on Azure).
            //var connectionString = configuration.GetConnectionString("ProdConnString");

            //Use this connection string when in development mode (when using local database).
            var connectionString = configuration.GetConnectionString("DevelopConnString");

            builder.UseSqlServer(connectionString);
            return new TravelPlanDbContext(builder.Options);
        }
    }
}
