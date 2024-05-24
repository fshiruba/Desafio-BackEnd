using Desafio_Backend.Areas.Rental.Pages;
using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Desafio_Backend.Services
{
    public class RentalService(
        RentalDbContext rentalDbContext,
        IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager) : IRentalService
    {
        public IHttpContextAccessor HttpContextAccessor { get; } = httpContextAccessor;

        public RentalDbContext RentalDbContext { get; } = rentalDbContext!;

        public UserManager<IdentityUser> UserManager { get; } = userManager;

        public async Task<DeliveryPerson> AddDeliveryPerson(DeliveryPerson newDeliveryPerson)
        {
            ArgumentNullException.ThrowIfNull(newDeliveryPerson);

            if (newDeliveryPerson.DateOfBirth == DateTime.MinValue)
            {
                newDeliveryPerson.DateOfBirth = null;
            }

            if (newDeliveryPerson.DateOfBirth.HasValue)
            {
                newDeliveryPerson.DateOfBirth = DateTime.SpecifyKind(newDeliveryPerson.DateOfBirth.Value, DateTimeKind.Utc);
            }

            if (!RentalDbContext.Database.IsRelational())
            {
                if (RentalDbContext.DeliveryPerson.Any(x => x.Cnpj == newDeliveryPerson.Cnpj))
                {
                    throw new ArgumentException("Cnpj is unique", nameof(newDeliveryPerson.Cnpj));
                }

                if (RentalDbContext.DeliveryPerson.Any(x => x.CnhNumber == newDeliveryPerson.CnhNumber))
                {
                    throw new ArgumentException("CnhNumber is unique", nameof(newDeliveryPerson.CnhNumber));
                }
            }

            await RentalDbContext.AddAsync(newDeliveryPerson);
            await RentalDbContext.SaveChangesAsync();

            return newDeliveryPerson;
        }

        public async Task<Rental> AddRental(IndexModel.RentalDTO rentalDto)
        {
            ArgumentNullException.ThrowIfNull(rentalDto);

            var plan = rentalDbContext.Plans.FirstOrDefault(x => x.Id == rentalDto.PlanId);
            if (plan == null) { throw new ArgumentException("Plan not found"); }

            var bike = rentalDbContext.Motorbikes.FirstOrDefault(x => x.Id == rentalDto.MotorbikeId);
            if (bike == null) { throw new ArgumentException("Motorbike not found"); }

            var deliveryPerson = rentalDbContext.DeliveryPerson.FirstOrDefault(x => x.Id == rentalDto.DeliveryPersonId);
            if (deliveryPerson == null) { throw new ArgumentException("DeliveryPerson not found"); }

            DateTime startdate = DateTime.Now.AddDays(1).Date;
            DateTime expectedenddate = DateTime.Now.AddDays(plan.RentalDays + 1).Date;

            var newRental = new Rental()
            {
                DeliveryPersonId = deliveryPerson.Id,
                DeliveryPerson = deliveryPerson,
                PlanId = plan.Id,
                Plan = plan,
                MotorbikeId = bike.Id,
                Motorbike = bike,
                StartDate = startdate,
                ExpectedEndDate = expectedenddate,
                EndDate = null,
            };

            DateTime.SpecifyKind(newRental.StartDate, DateTimeKind.Utc);
            DateTime.SpecifyKind(newRental.ExpectedEndDate, DateTimeKind.Utc);

            newRental.EndDate = null;

            await RentalDbContext.AddAsync(newRental);
            await RentalDbContext.SaveChangesAsync();

            return newRental;
        }

        public decimal CalculateCost(DateTime startDate, DateTime expectedEndDate, DateTime? endDate, int planId)
        {
            var plan = rentalDbContext.Plans.FirstOrDefault(x => x.Id == planId);

            ArgumentNullException.ThrowIfNull(plan);

            var checkPenalty = endDate.HasValue;
            var effectiveDate = endDate ?? expectedEndDate;
            var diff = (effectiveDate - startDate).Days;

            decimal cost;

            if (checkPenalty)
            {
                if (diff < plan.RentalDays)
                {
                    // charge for the rental days
                    cost = diff * plan.RentalCostPerDay;

                    // charge the penalty based on "unused" rental days
                    decimal percent = 1 + (plan.PenaltyFeePercent / 100m);

                    cost += (plan.RentalDays - diff) * plan.RentalCostPerDay * percent;

                    // convert from cents
                    cost /= 10m;
                }
                else if (diff > plan.RentalDays)
                {
                    // charge for the rental days
                    cost = diff * plan.RentalCostPerDay;

                    // charge an extra fee for each day over the limit
                    cost += (diff - plan.RentalDays) * 500;

                    // convert from cents
                    cost /= 10m;
                }
                else
                {
                    // charge for the rental days
                    cost = diff * plan.RentalCostPerDay;

                    // convert from cents
                    cost /= 10m;
                }
            }
            else
            {
                // charge for the rental days
                cost = diff * plan.RentalCostPerDay;

                // convert from cents
                cost /= 10m;
            }

            return cost;
        }

        /// <summary>
        /// This should definetely be changed to a combo of ENUM and tables
        /// </summary>
        public bool DeliveryPersonHasLicense(ClaimsPrincipal user, CnhHelper.CnhType cnh)
        {
            ArgumentNullException.ThrowIfNull(user);

            var id = UserManager.GetUserId(user);
            var deliveryPerson = rentalDbContext.DeliveryPerson.FirstOrDefault(x => x.IdentityUserId == id);

            if (deliveryPerson == null)
            {
                return false;
            }

            return cnh.DisplayName switch
            {
                "A" => deliveryPerson.CnhType == CnhHelper.A.Value || deliveryPerson.CnhType == CnhHelper.AB.Value,
                "B" => deliveryPerson.CnhType == CnhHelper.B.Value || deliveryPerson.CnhType == CnhHelper.AB.Value,
                "AB" => deliveryPerson.CnhType == CnhHelper.AB.Value,
                _ => false,
            };
        }

        public int GetDeliveryPersonId(ClaimsPrincipal user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var id = UserManager.GetUserId(user);
            var deliveryPerson = rentalDbContext.DeliveryPerson.FirstOrDefault(x => x.IdentityUserId == id);

            if (deliveryPerson == null)
            {
                return 0;
            }

            return deliveryPerson.Id;
        }
    }
}