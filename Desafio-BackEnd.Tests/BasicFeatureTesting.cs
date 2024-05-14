using Desafio_Backend.Areas.Admin.Pages;
using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace Desafio_BackEnd.Tests
{
    

    public class BasicFeatureTesting
    {
        public AdminService MockAdminService(RentalDbContext context, string AdminId)
        {
            var QueueService = new QueueService<Motorbike>();

            var mockHttp = new Mock<IHttpContextAccessor>();
            mockHttp.Setup(x => x.HttpContext).Returns(new Mock<HttpContext>().Object);
            mockHttp.Setup(x => x.HttpContext!.User).Returns(new Mock<ClaimsPrincipal>().Object);

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser { Id = AdminId });

            var adminService = new AdminService(context, QueueService, mockHttp.Object, mockUserManager.Object);

            return adminService;
        }


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
                ProductionYear = 2024
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
                ProductionYear = 2000
            };

            var missingPlate = new Motorbike
            {
                Id = 0,
                Model = "TEST",
                ProductionYear = 2000
            };

            var missingYear = new Motorbike
            {
                Id = 0,
                Model = "TEST",
                Plate = "XXXXXX1"                
            };

            var adminService =  MockAdminService(context,nameof(AdminCanAddNewMotorbikes_MissingRequiredFields));

            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingModel));
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingPlate));
            await Assert.ThrowsAnyAsync<Exception>(async () => await adminService.AddMotorbike(missingYear));
        }
    }
}