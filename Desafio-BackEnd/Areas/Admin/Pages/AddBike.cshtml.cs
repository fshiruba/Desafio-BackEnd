using Desafio_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio_Backend.Areas.Admin.Pages
{
    public class AddBikeModel : PageModel
    {
        public string ReturnUrl { get; set; }

        [BindProperty]
        public Motorbike NewBike { get; set; }

        public void OnGet()
        {
        }

        public async Task<object?> OnPostAsync()
        {
            throw new NotImplementedException();
        }
    }
}
