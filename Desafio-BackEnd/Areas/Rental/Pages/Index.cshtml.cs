using Desafio_Backend.Data;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Desafio_Backend.Areas.Rental.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly RentalDbContext rentalDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IRentalService rentalService;

        public IndexModel(IRentalService rentalService, ILogger<IndexModel> logger, RentalDbContext rentalDbContext, UserManager<IdentityUser> userManager)
        {
            this.rentalService = rentalService;
            this.logger = logger;
            this.rentalDbContext = rentalDbContext;
            this.userManager = userManager;
        }

        public List<SelectListItem> Bikes { get; set; }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public RentalDTO NewRental { get; set; }

        public List<SelectListItem> Plans { get; set; }

        public decimal EstimatedCost { get; set; }

        public List<SelectListItem> GetBikes()
        {
            return rentalDbContext.Motorbikes
                .Where(x => x.Rentals.Count == 0 || x.Rentals.All(x => x.EndDate.HasValue))
                .Select(x => new SelectListItem($"{x.Model} - {x.ProductionYear} - {x.Plate}", x.Id.ToString()))
                .ToList();
        }

        public List<SelectListItem> GetPlans()
        {
            return rentalDbContext.Plans.Select(x => new SelectListItem($"{x.RentalDays} dias - R$ {x.RentalCostPerDay / 10} por dia - {x.PenaltyFeePercent}% de multa", x.Id.ToString())).ToList();
        }

        public async Task OnGetAsync(string returnUrl)
        {
            Bikes = GetBikes();
            Bikes[0].Selected = true;

            Plans = GetPlans();
            Plans[0].Selected = true;

            NewRental = new RentalDTO
            {
                StartDate = DateTime.Now.AddDays(1).Date,
                ExpectedEndDate = DateTime.Now.AddDays(8).Date,
                DeliveryPersonId = rentalService.GetDeliveryPersonId(User),
            };

            ReturnUrl = returnUrl;

            EstimatedCost = rentalService.CalculateCost(NewRental.StartDate, NewRental.ExpectedEndDate, null, int.Parse(Plans[0].Value));
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ArgumentNullException.ThrowIfNull(NewRental);

            var userId = userManager.GetUserId(User);
            var deliveryPerson = rentalDbContext.DeliveryPerson.FirstOrDefault(x => x.IdentityUserId == userId);
            NewRental.DeliveryPersonId = deliveryPerson!.Id;


            if (ModelState.IsValid)
            {
                await rentalService.AddRental(NewRental);
            }

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> OnPostChangeAsync([FromBody] PlanDto body)
        {
            var plan = rentalDbContext.Plans.FirstOrDefault(x => x.Id == body.PlanId);

            if (plan == null)
            {
                return new BadRequestResult();
            }

            var startDateRaw = DateTime.Now.AddDays(1);
            var endDateRaw = DateTime.Now.AddDays(plan.RentalDays + 1);
            var startDate = startDateRaw.ToString("yyyy-MM-dd");
            var endDate = endDateRaw.ToString("yyyy-MM-dd");
            EstimatedCost = rentalService.CalculateCost(startDateRaw, endDateRaw, null, plan.Id);
            var estimatedCostString = EstimatedCost.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            return new JsonResult(new { startDate, endDate, estimatedCostString });
        }

        public class PlanDto()
        {
            [JsonPropertyName("planid")]
            public int PlanId { get; set; }
        }

        public class RentalDTO
        {
            private DateTime expectedEndDate;
            private DateTime startDate;

            public int DeliveryPersonId { get; set; }

            public DateTime ExpectedEndDate { get => expectedEndDate.Date; set => expectedEndDate = value.Date; }

            public int MotorbikeId { get; set; }

            public int PlanId { get; set; }

            public DateTime StartDate { get => startDate.Date; set => startDate = value.Date; }
        }
    }
}