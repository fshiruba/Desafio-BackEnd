using Desafio_Backend.Data;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Desafio_Backend.Areas.Rental.Pages
{
    public class ListRentalsModel : PageModel
    {
        private readonly RentalDbContext rentalDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IRentalService rentalService;

        public ListRentalsModel(RentalDbContext rentalDbContext, UserManager<IdentityUser> userManager, IRentalService rentalService)
        {
            this.rentalDbContext = rentalDbContext;
            this.userManager = userManager;
            this.rentalService = rentalService;
        }

        public List<Models.Rental> Rentals { get; set; }

        public string ReturnUrl { get; set; }

        public string CalculateCost(Models.Rental rental)
        {
            var cost = rentalService.CalculateCost(rental.StartDate, rental.ExpectedEndDate, rental.EndDate, rental.PlanId);
            return cost.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        public async Task OnGetAsync(string returnUrl)
        {
            ReturnUrl = returnUrl;

            var userId = userManager.GetUserId(User);

            if (User.IsInRole("Admin"))
            {
                Rentals = rentalDbContext.Rentals.ToList();
            }
            else if (User.IsInRole("DeliveryPerson"))
            {
                Rentals = rentalDbContext.Rentals
                    .Include(x => x.DeliveryPerson)
                    .Include(x => x.Motorbike)
                    .Include(x => x.Plan)
                    .Where(x => x.DeliveryPerson != null && x.DeliveryPerson.IdentityUserId == userId)
                    .ToList();
            }
        }
    }
}