using Desafio_Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Desafio_Backend.Data
{
    public class RentalDbContext(DbContextOptions<RentalDbContext> options) : DbContext(options)
    {
        public DbSet<DeliveryPerson> DeliveryPerson { get; set; }

        public DbSet<MotorbikeLog> MotorbikeLogs { get; set; }

        public DbSet<Motorbike> Motorbikes { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TESTING ONLY - REMOVE IN PROD
            modelBuilder.Entity<Motorbike>().HasData(
            new Motorbike { Model = "YAMAHA", Plate = "ABC1234", ProductionYear = 2000, AdminId = "SYSTEM", Id = 1 },
            new Motorbike { Model = "HONDA", Plate = "DEF5678", ProductionYear = 2010, AdminId = "SYSTEM", Id = 2 });

            // 07 dias com um custo de R$30,00 por dia
            // 15 dias com um custo de R$28,00 por dia
            // 30 dias com um custo de R$22,00 por dia
            // 45 dias com um custo de R$20,00 por dia
            // 50 dias com um custo de R$18,00 por dia

            modelBuilder.Entity<Plan>().HasData(
            new Plan { Id = 1, RentalDays = 7, RentalCostPerDay = 300, PenaltyFeePercent = 20 },
            new Plan { Id = 2, RentalDays = 15, RentalCostPerDay = 280, PenaltyFeePercent = 40 },
            new Plan { Id = 3, RentalDays = 30, RentalCostPerDay = 220, PenaltyFeePercent = 40 },
            new Plan { Id = 4, RentalDays = 45, RentalCostPerDay = 200, PenaltyFeePercent = 40 },
            new Plan { Id = 5, RentalDays = 50, RentalCostPerDay = 180, PenaltyFeePercent = 40 });

            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");



        }
    }
}