using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Desafio_Backend.Areas.Rental.Pages
{
    public class EditRentalModel : PageModel
    {
        private readonly RentalDbContext rentalDbContext;
        private readonly IRentalService rentalService;

        public EditRentalModel(RentalDbContext rentalDbContext, IRentalService rentalService)
        {
            this.rentalDbContext = rentalDbContext;
            this.rentalService = rentalService;
        }

        public decimal EstimatedCost { get; set; }

        public string PictureUrl { get; set; }

        public List<Plan> Plans { get; set; }

        [BindProperty]
        public Models.Rental Rental { get; set; }

        public string ReturnUrl { get; set; }

        public List<SelectListItem> GetPlans(int selectedId = 0)
        {
            return Plans.ConvertAll(x => new SelectListItem($"{x.RentalDays} dias - R$ {x.RentalCostPerDay / 10} por dia - {x.PenaltyFeePercent}% de multa", x.Id.ToString(), x.Id == selectedId));
        }

        public IActionResult OnGet(int id, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Rental = rentalDbContext.Rentals
                    .Include(x => x.DeliveryPerson)
                    .Include(x => x.Motorbike)
                    .Include(x => x.Plan)
                    .FirstOrDefault(x => x.Id == id)!;

            Plans = rentalDbContext.Plans.ToList();

            if (Rental == null)
            {
                return Redirect(returnUrl);
            }

            if (Rental.DeliveryPerson.CnhPicture != null)
            {
                PictureUrl = "https://" + Request.Host + "/uploads/" + Rental.DeliveryPerson.CnhPicture.Split("uploads\\")[1];
            }

            EstimatedCost = rentalService.CalculateCost(Rental.StartDate, Rental.ExpectedEndDate, Rental.EndDate, Rental.PlanId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            Rental.Motorbike = null;
            Rental.Plan = null;
            Rental.DeliveryPerson = null;

            rentalDbContext.Rentals.Attach(Rental).State = EntityState.Modified;
            await rentalDbContext.SaveChangesAsync();

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> OnPostUpdateAsync([FromBody] RentalDto body)
        {
            var plan = rentalDbContext.Plans.FirstOrDefault(x => x.Id == body.PlanId);

            if (plan == null)
            {
                return new BadRequestResult();
            }

            EstimatedCost = rentalService.CalculateCost(body.GetStartDate(), body.GetExpectedEndDate(), body.GetEndDate(), plan.Id);

            var estimatedCostString = EstimatedCost.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            return new JsonResult(new { estimatedCostString });
        }

        public class RentalDto()
        {
            [JsonPropertyName("enddate")]
            public string EndDate { get; set; }

            [JsonPropertyName("expectedenddate")]
            public string ExpectedEndDate { get; set; }

            [JsonPropertyName("planid")]
            public int PlanId { get; set; }

            [JsonPropertyName("startdate")]
            public string StartDate { get; set; }

            public DateTime? GetEndDate() => string.IsNullOrEmpty(EndDate) ? null : DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("pt-BR"));

            public DateTime GetExpectedEndDate() => DateTime.ParseExact(ExpectedEndDate, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("pt-BR"));

            public DateTime GetStartDate() => DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("pt-BR"));
        }
    }
}