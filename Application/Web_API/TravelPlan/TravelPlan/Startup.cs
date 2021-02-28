using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TravelPlan.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Repository;
using AutoMapper;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.Services;
using TravelPlan.DTOs.Profiles;
using Newtonsoft.Json;
using TravelPlan.Services.AuthentificationService;
using TravelPlan.Services.BusinessLogicServices;
using Microsoft.AspNetCore.Http;
using TravelPlan.Services.MessagingService;
using TravelPlan.Services.RedisServices;
using TravelPlan.Helpers;

namespace TravelPlan
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEditRightsService, EditRightsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<ITripService, TripService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IAccommodationService, AccommodationService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IVoteService, VoteService>();
            services.AddTransient<IAddOnService, AddOnService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRedisConnectionBuilder, RedisConnectionBuilder>();
            services.AddAutoMapper(typeof(UserProfiles));
            services.AddControllers().AddMvcOptions(x => x.Filters.Add(new AuthorizeAttribute()));
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((host) => true).AllowCredentials();
                });
            });
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<RedisAppSettings>(Configuration.GetSection("Redis"));
            services.AddDistributedRedisCache(redis =>
                { 
                    redis.Configuration = Configuration["Redis:ConnectionString"]; 
                }
            );


            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<TravelPlanDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ProdConnString")), ServiceLifetime.Transient);
            }
            else if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                services.AddDbContext<TravelPlanDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DevelopConnString")), ServiceLifetime.Transient);
            }

            //NOTE: The following lines can be used to apply migrations during runtime.
            //TODO: Check if this code has any purpose when creating migrations in the DataAccess class library.

            //ServiceProvider sp = services.BuildServiceProvider();
            //WeatherForecastContext wfc = sp.GetService<WeatherForecastContext>();
            //wfc.Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CORS");

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("travel-plan-hub");
            });
        }
    }
}
