using Desafio_Backend.Models;

namespace Desafio_Backend.Services
{
    public interface IAdminService
    {
        Task<Motorbike> AddMotorbike(Motorbike newBike);

        Task<Motorbike?> GetMotorbike(int id);

        Task RemoveMotorbike(Motorbike bike);

        Task<Motorbike> UpdateMotorbike(Motorbike editedBike);
    }
}