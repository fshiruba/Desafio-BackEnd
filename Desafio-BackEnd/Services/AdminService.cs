using Desafio_Backend.Data;
using Desafio_Backend.Models;

namespace Desafio_Backend.Services
{
    public class AdminService(RentalDbContext rentalDbContext) : IAdminService
    {
        public RentalDbContext RentalDbContext { get; } = rentalDbContext;

        public async Task<Motorbike> AddMotorbike(Motorbike newBike)
        {

            ArgumentNullException.ThrowIfNull(newBike);
            await RentalDbContext.AddAsync(newBike);

            await RentalDbContext.SaveChangesAsync();

            return newBike;
        }
    }
}
