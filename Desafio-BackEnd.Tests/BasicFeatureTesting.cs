using Desafio_Backend.Areas.Admin.Pages;
using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Desafio_BackEnd.Tests
{
    public class BasicFeatureTesting
    {
        [Fact]
        public async Task AdminCanAddNewMotorbikes_GoodData()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<RentalDbContext>()
           .UseInMemoryDatabase(databaseName: "MockDB")
           .Options;

            // The test db (in memory) IS NOT a postgresdb
            using var context = new RentalDbContext(dbOptions);

            var newBike = new Motorbike
            {
                Model="TEST",
                Plate="XXXXXXX",
                ProductionYear=2000
            };

            var adminService = new AdminService(context);

            // Act
            Motorbike result = await adminService.AddMotorbike(newBike);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task AdminCanAddNewMotorbikes_MissingRequiredFields()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<RentalDbContext>()
           .UseInMemoryDatabase(databaseName: "MockDB")
           .Options;

            // The test db (in memory) IS NOT a postgresdb
            using var context = new RentalDbContext(dbOptions);

            var missingModel = new Motorbike
            {                
                Plate = "XXXXXX0",
                ProductionYear = 2000
            };

            var missingPlate = new Motorbike
            {
                Model = "TEST",
                ProductionYear = 2000
            };

            var missingYear = new Motorbike
            {
                Model = "TEST",
                Plate = "XXXXXX1"                
            };

            var adminService = new AdminService(context);

            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingModel));
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingPlate));
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingYear));

            
        }
    }
}