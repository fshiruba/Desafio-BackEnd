using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio_Backend.Areas.Admin.Pages
{
    public class IndexModel(RentalDbContext ctx, IAdminService adminService) : PageModel
    {
        private readonly RentalDbContext context = ctx;
        private readonly IAdminService adminService = adminService;

        public List<Motorbike> Motorbikes { get; set; }

        public void OnGet()
        {
            Motorbikes = [.. context.Motorbikes];
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var bike = await adminService.GetMotorbike(id);

            if (bike != null)
            {
                await adminService.RemoveMotorbike(bike);
            }

            Motorbikes = [.. context.Motorbikes];

            return Page();
        }
    }
}
