using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Services
{
    public class AdminService(
        RentalDbContext rentalDbContext,
        IQueueService<Motorbike> queueService,
        IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager) : IAdminService
    {
        public IHttpContextAccessor HttpContextAccessor { get; } = httpContextAccessor;

        public IQueueService<Motorbike> QueueService { get; } = queueService;

        public RentalDbContext RentalDbContext { get; } = rentalDbContext!;

        public UserManager<IdentityUser> UserManager { get; } = userManager;

        public async Task<Motorbike> AddMotorbike(Motorbike newBike)
        {
            ArgumentNullException.ThrowIfNull(newBike);
            ArgumentException.ThrowIfNullOrEmpty(newBike.Plate);
            ArgumentException.ThrowIfNullOrEmpty(newBike.Model);

            if (newBike.ProductionYear <= 0)
            {
                throw new ArgumentException("ProductionYear <= 0", nameof(newBike.ProductionYear));
            }

            if (string.IsNullOrEmpty(newBike.AdminId))
            {
                var user = await UserManager.GetUserAsync(HttpContextAccessor!.HttpContext!.User);
                newBike.AdminId = user!.Id;
            }

            if (!RentalDbContext.Database.IsRelational() && RentalDbContext.Motorbikes.Any(x => x.Plate == newBike.Plate))
            {
                throw new ArgumentException("Plates are unique", nameof(newBike.Plate));
            }

            newBike.Model = newBike.Model.ToUpper();
            newBike.Plate = newBike.Plate.ToUpper();

            await RentalDbContext.AddAsync(newBike);
            await RentalDbContext.SaveChangesAsync();

            QueueService.Publish(newBike);

            return newBike;
        }

        public async Task<Motorbike?> GetMotorbike(int id) => await RentalDbContext.Motorbikes.FirstOrDefaultAsync(x => x.Id == id);

        public async Task RemoveMotorbike(Motorbike bike)
        {
            ArgumentNullException.ThrowIfNull(bike);

            rentalDbContext.Remove(bike);
            await RentalDbContext.SaveChangesAsync();
        }

        public async Task<Motorbike> UpdateMotorbike(Motorbike editedBike)
        {
            ArgumentNullException.ThrowIfNull(editedBike);
            ArgumentException.ThrowIfNullOrEmpty(editedBike.Plate);
            ArgumentException.ThrowIfNullOrEmpty(editedBike.Model);

            if (editedBike.ProductionYear <= 0)
            {
                throw new ArgumentException("ProductionYear <= 0", nameof(editedBike.ProductionYear));
            }

            var user = await UserManager.GetUserAsync(HttpContextAccessor!.HttpContext!.User);
            editedBike.AdminId = user!.Id;

            if (!RentalDbContext.Database.IsRelational() && RentalDbContext.Motorbikes.Any(x => x.Plate == editedBike.Plate))
            {
                throw new ArgumentException("Plates are unique", nameof(editedBike.Plate));
            }

            editedBike.Model = editedBike.Model.ToUpper();
            editedBike.Plate = editedBike.Plate.ToUpper();

            RentalDbContext.Motorbikes.Update(editedBike);
            await RentalDbContext.SaveChangesAsync();

            return editedBike;
        }
    }
}