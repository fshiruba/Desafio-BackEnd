using Desafio_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Data
{
    public class RentalDbContext(DbContextOptions<RentalDbContext> options) : DbContext(options) //, IConfiguration configuration
    {
        //private readonly IConfiguration configuration = configuration;

        public DbSet<DeliveryPerson> DeliveryPerson { get; set; }

        public DbSet<Motorbike> Motorbikes { get; set; }

        public DbSet<MotorbikeLog> MotorbikeLogs { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motorbike>().HasData(
            new Motorbike { Id = 1, AdminId = "SYSTEM", Model = "YAMAHA", ProductionYear = 2000, Plate = "ABC-1234", },
            new Motorbike { Id = 2, AdminId = "SYSTEM", Model = "HONDA", ProductionYear = 2010, Plate = "DEF-5678", });
        }
    }
}