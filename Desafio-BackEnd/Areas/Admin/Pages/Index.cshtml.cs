using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio_Backend.Areas.Admin.Pages
{
    public class IndexModel(RentalDbContext ctx) : PageModel
    {
        private readonly RentalDbContext context = ctx;

        public List<Motorbike> Motorbikes { get; set; }

        public void OnGet()
        {
            Motorbikes = [.. context.Motorbikes];
        }
    }
}
