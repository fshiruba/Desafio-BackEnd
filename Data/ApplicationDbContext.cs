using Desafio_Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Desafio_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IConfiguration configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }

    public class RentalDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public RentalDbContext(DbContextOptions<RentalDbContext> options, IConfiguration configuration)
          : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<DeliveryPerson> DeliveryPerson { get; set; }
        public DbSet<Motorbike> Motorbikes { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }


}
