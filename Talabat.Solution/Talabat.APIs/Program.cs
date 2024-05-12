using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Error;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helper;
using Talabat.APIs.Middlewares;
using Talabat.Core.Models.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services - Add services to the container.
            // Add services to the container.
            builder.Services.AddControllers(); // Register web Api service to DI container
           

            builder.Services.AddSwagerServices();

            // Add DbContext Service
            builder.Services.AddDbContext<StoreContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Add IdentityContect services
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            // Allow DI For Redis connection
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var redisConnection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(redisConnection);
            });
            // Application Services
            builder.Services.AddAplicationServices();
            // Identity Services
            builder.Services.AddIdentityServices(builder.Configuration);
            // Cors Servic
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyOptions =>
                {
                    policyOptions.AllowAnyHeader();
                    policyOptions.AllowAnyMethod();
                    policyOptions.WithOrigins(builder.Configuration["frontBaseUrl"]);
                });
            });
          


            #endregion

            // 2. Build Project
            var app = builder.Build();

            #region Update-Database - Data Seed
            // To update database [apply migration]

            // Ask CLR Explicitly to create object from StoreContext 
            // 1. creates a new service scope within the application's service container.
            // services are typically registered and resolved within a service container.
            using var Scope = app.Services.CreateScope();
            // 2. This line retrieves the service provider from the created scope.
            // The service provider is responsible for resolving dependencies requested by the application.
            var Service = Scope.ServiceProvider;
            // 3. This line retrieves an instance of StoreContext - AppIdentityDbContext
            var DbContext = Service.GetRequiredService<StoreContext>();
            var IdentityDbContext = Service.GetRequiredService<AppIdentityDbContext>();
            // Logger Factory
            var LoggerFactory = Service.GetRequiredService<ILoggerFactory>();
            try
            {
                await DbContext.Database.MigrateAsync();
                await IdentityDbContext.Database.MigrateAsync();
                // Seed Data
                await StoreContextSeed.SeedAsync(DbContext);
                // User Seed
                var userManager = Service.GetRequiredService<UserManager<AppUser>>();
                await IdentityContextSeed.SeedUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Has Occured During apply migartion");
            }

            #endregion

            #region Configure Kestrel Middlewares
            // Add custom exception middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Add Swager Moddlewares
                app.UseSwagerMiddlewares();
             
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // to redirect to error controller in some fail scenarios [NotFound endpoint]
            app.UseHttpsRedirection();

            app.UseStaticFiles(); // serve static file (wwwroot)
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers(); // routing to direct incoming HTTP requests to controller action methods based on the request's URL and HTTP method.
            #endregion

            app.Run();
        }
    }
}
