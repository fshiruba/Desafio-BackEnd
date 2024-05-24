using System.Security.Claims;
using Desafio_Backend.Areas.Rental.Pages;
using Desafio_Backend.Models;

namespace Desafio_Backend.Services
{
    public interface IRentalService
    {
        Task<DeliveryPerson> AddDeliveryPerson(DeliveryPerson newDeliveryPerson);

        Task<Rental> AddRental(IndexModel.RentalDTO newRental);

        decimal CalculateCost(DateTime startDate, DateTime expectedEndDate, DateTime? endDate, int planId);

        bool DeliveryPersonHasLicense(ClaimsPrincipal user, CnhHelper.CnhType cnh);

        int GetDeliveryPersonId(ClaimsPrincipal user);
    }
}