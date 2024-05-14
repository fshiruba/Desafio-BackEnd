using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Desafio_Backend.Services
{
    public class AdminService(RentalDbContext rentalDbContext, IQueueService<Motorbike> queueService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : IAdminService
    {
        public RentalDbContext RentalDbContext { get; } = rentalDbContext;

        public IQueueService<Motorbike> QueueService { get; } = queueService;

        public IHttpContextAccessor HttpContextAccessor { get; } = httpContextAccessor;

        public UserManager<IdentityUser> UserManager { get; } = userManager;

        public async Task<Motorbike> AddMotorbike(Motorbike newBike)
        {
            ArgumentNullException.ThrowIfNull(newBike);
            ArgumentNullException.ThrowIfNullOrEmpty(newBike.Plate);
            ArgumentNullException.ThrowIfNullOrEmpty(newBike.Model);

            if (newBike.ProductionYear <= 0)
            {
                throw new ArgumentException("ProductionYear <= 0", nameof(newBike.ProductionYear));
            }


            var user = await UserManager.GetUserAsync(HttpContextAccessor!.HttpContext!.User);
            newBike.AdminId = user!.Id;

            await RentalDbContext.AddAsync(newBike);
            await RentalDbContext.SaveChangesAsync();

            QueueService.Publish(newBike);

            return newBike;
        }
    }
}
