using System.Runtime.CompilerServices;
using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddDbContext<RentalDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IRentalService, RentalService>();
            builder.Services.AddSingleton<IQueueService<Motorbike>, QueueService<Motorbike>>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Seed data during application startup
            using (var scope = app.Services.CreateScope())
            {
                // Roles
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                SeedRoles(roleManager).GetAwaiter().GetResult();

                var rentalContext = scope.ServiceProvider.GetRequiredService<RentalDbContext>();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            app.Run();
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("DeliveryPerson"))
            {
                await roleManager.CreateAsync(new IdentityRole("DeliveryPerson"));
            }
        }
    }
}
