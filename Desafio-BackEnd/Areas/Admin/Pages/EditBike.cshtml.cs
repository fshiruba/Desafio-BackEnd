using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio_Backend.Areas.Admin.Pages
{
    public class EditBikeModel : PageModel
    {
        public EditBikeModel(IAdminService adminService)
        {
            AdminService = adminService;
        }

        public IAdminService AdminService { get; }

        [BindProperty]
        public Motorbike EditedBike { get; set; }

        public int Id { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, string returnUrl = "/Admin")
        {
            Id = id;
            ReturnUrl = returnUrl;

            var getBike = await AdminService.GetMotorbike(id);

            if (getBike == null)
            {
                return LocalRedirect(ReturnUrl);
            }

            EditedBike = getBike;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] string returnUrl = "/Admin")
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid && EditedBike != null)
            {
                EditedBike = await AdminService.UpdateMotorbike(EditedBike);
            }

            return LocalRedirect(ReturnUrl);
        }
    }
}