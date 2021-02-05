using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.DataAccess
{
    public class TravelPlanDbContext : DbContext
    {
        public TravelPlanDbContext(DbContextOptions<TravelPlanDbContext> options) : base(options)
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
        public DbSet<OtherAddOn> OtherAddOns { get; set; }
        public DbSet<TripType> TripTypes { get; set; }
        public DbSet<SeaType> SeaTypes { get; set; }
        public DbSet<WinterType> WinterTypes { get; set; }
        public DbSet<SpaType> SpaTypes { get; set; }
        public DbSet<OtherType> OtherTypes { get; set; }
        public DbSet<SeaDecorator> SeaDecorators { get; set; }
        public DbSet<Aquapark> Aquaparks { get; set; }
        public DbSet<Waterboard> Waterboards { get; set; }
        public DbSet<SunBeds> SunBeds { get; set; }
        public DbSet<Cruise> Cruises { get; set; }
        public DbSet<CruiseDecorator> CruiseDecorators { get; set; }
        public DbSet<Breakfast> Breakfasts { get; set; }
        public DbSet<Lunch> Lunches { get; set; }
        public DbSet<BreakfastDecorator> BreakfastDecorators { get; set; }
        public DbSet<Coffee> Coffees { get; set; }
        public DbSet<Tea> Teas { get; set; }
        public DbSet<Juice> Juices { get; set; }
        public DbSet<LunchDecorator> LunchDecorators { get; set; }
        public DbSet<Wine> Wines { get; set; }
        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<WinterDecorator> WinterDecorators { get; set; }
        public DbSet<SkiPass> SkiPasses { get; set; }
        public DbSet<SkiEquipment> SkiEquipment { get; set; }
        public DbSet<SkiEquipmentDecorator> SkiEquipmentDecorators { get; set; }
        public DbSet<Snowboard> Snowboards { get; set; }
        public DbSet<Skis> Skis { get; set; }
        public DbSet<SkiPoles> SkiPoles { get; set; }
        public DbSet<SkiBoots> SkiBoots { get; set; }
        public DbSet<Sled> Sleds { get; set; }
        public DbSet<SpaDecorator> SpaDecorators { get; set; }
        public DbSet<BikeRent> BikeRents { get; set; }
        public DbSet<ScooterRent> ScooterRents { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<TrainTour> TrainTours { get; set; }
        public DbSet<WalkDecorator> WalkDecorators { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<TrainTourDecorator> TrainTourDecorators { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealDecorator> MealDecorators { get; set; }
        public DbSet<Pogaca> Pogace { get; set; }
        public DbSet<Schnapps> Schnapps { get; set; }
        public DbSet<Notification> Notifications { get; set; }

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

            modelBuilder.Entity<AddOn>()
                .HasOne(a => a.Votable)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SeaDecorator>()
                .HasOne(a => a.Decorator)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WinterDecorator>()
                .HasOne(a => a.Decorator)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SpaDecorator>()
                .HasOne(a => a.Decorator)
                .WithMany()
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
