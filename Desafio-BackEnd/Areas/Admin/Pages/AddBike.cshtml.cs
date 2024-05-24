using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio_Backend.Areas.Admin.Pages
{
    public class AddBikeModel : PageModel
    {
        public AddBikeModel(IAdminService adminService)
        {
            AdminService = adminService;
        }

        public IAdminService AdminService { get; }

        [BindProperty]
        public Motorbike NewBike { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = "/Admin")
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] string returnUrl = "/Admin")
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                await AdminService.AddMotorbike(NewBike);
            }

            return LocalRedirect(ReturnUrl);
        }
    }
}