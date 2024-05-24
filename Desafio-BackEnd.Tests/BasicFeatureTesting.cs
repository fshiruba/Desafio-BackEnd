using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Moq;

namespace Desafio_BackEnd.Tests
{
    public class BasicFeatureTesting
    {
        [Fact]
        public async Task AdminCanAddNewMotorbikes_GoodData()
        {
            // Arrange - DB
            var dbOptions = new DbContextOptionsBuilder<RentalDbContext>()
            .UseInMemoryDatabase(databaseName: "MockDB")
            .Options;

            // The test db (in memory) IS NOT a postgresdb
            using var context = new RentalDbContext(dbOptions);

            var newBike = new Motorbike
            {
                Id = 0,
                Model = "TEST",
                Plate = "XXXXXXX",
                ProductionYear = 2024,
                AdminId = "AdminCanAddNewMotorbikes_GoodData"
            };

            // Arrange - Service
            var adminService = MockAdminService(context, nameof(AdminCanAddNewMotorbikes_GoodData));

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
                Id = 0,
                Plate = "XXXXXX0",
                Model = null!,
                ProductionYear = 2000,
                AdminId = "AdminCanAddNewMotorbikes_MissingRequiredFields"
            };

            var missingPlate = new Motorbike
            {
                Id = 0,
                Plate = null!,
                Model = "TEST",
                ProductionYear = 2000,
                AdminId = "AdminCanAddNewMotorbikes_MissingRequiredFields"
            };

            var missingYear = new Motorbike
            {
                Id = 0,
                Plate = "XXXXXX1",
                Model = "TEST",
                ProductionYear = 0,
                AdminId = "AdminCanAddNewMotorbikes_MissingRequiredFields"
            };

            var adminService = MockAdminService(context, nameof(AdminCanAddNewMotorbikes_MissingRequiredFields));

            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingModel));
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingPlate));
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingYear));
        }

        /// <summary>
        /// NOTE: InMemoryDB IS NOT RELATIONAL, so Unique Indexes WILL fail
        /// </summary>
        [Fact]
        public async Task AdminCanAddNewMotorbikes_PlateIsUnique()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<RentalDbContext>()
            .UseInMemoryDatabase(databaseName: "MockDB")
            .Options;

            // The test db (in memory) IS NOT a postgresdb
            using var context = new RentalDbContext(dbOptions);

            var newBike1 = new Motorbike
            {
                Id = 0,
                Model = "TEST",
                Plate = "XXXXXXX",
                ProductionYear = 2024,
                AdminId = "AdminCanAddNewMotorbikes_PlateIsUnique"
            };

            var newBike2 = new Motorbike
            {
                Id = 0,
                Model = "TEST",
                Plate = "XXXXXXX",
                ProductionYear = 2024,
                AdminId = "AdminCanAddNewMotorbikes_PlateIsUnique"
            };

            var adminService = MockAdminService(context, nameof(AdminCanAddNewMotorbikes_MissingRequiredFields));

            // Act
            var result = await adminService.AddMotorbike(newBike1);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(newBike2));
        }

        public AdminService MockAdminService(RentalDbContext context, string AdminId)
        {
            var QueueService = new QueueService<Motorbike>();

            var mockHttp = new Mock<IHttpContextAccessor>();
            mockHttp.Setup(x => x.HttpContext).Returns(new Mock<HttpContext>().Object);
            mockHttp.Setup(x => x.HttpContext!.User).Returns(new Mock<ClaimsPrincipal>().Object);

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser { Id = AdminId });

            return new AdminService(context, QueueService, mockHttp.Object, mockUserManager.Object);
        }
    }
}