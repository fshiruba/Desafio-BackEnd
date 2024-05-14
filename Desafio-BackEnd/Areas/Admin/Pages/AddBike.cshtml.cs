using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio_Backend.Areas.Admin.Pages
{
    public class AddBikeModel : PageModel
    {
        public AddBikeModel(IAdminService adminService)
        {
            //UserManager = userManager;
            AdminService = adminService;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public Motorbike NewBike { get; set; }

        //public UserManager<IdentityUser> UserManager { get; }
        public IAdminService AdminService { get; }

        public async Task OnGetAsync(string returnUrl = null)
        {            
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //var user = await UserManager.GetUserAsync(HttpContext.User);
            //NewBike.AdminId = user!.Id;

            if (ModelState.IsValid)
            {
                await AdminService.AddMotorbike(NewBike);
            }

            return LocalRedirect(ReturnUrl);
        }
    }
}