using System.Text;
using Desafio_Backend.Models;
using Desafio_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Areas.Rental.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IRentalService _rentalService;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IUserStore<IdentityUser> _userStore;

        [BindProperty]
        public IFormFile? CnhPicture { get; set; }

        public List<SelectListItem> CnhTypes { get; set; }

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            IRentalService rentalService,
            ILogger<RegisterModel> logger,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _environment = environment;
            _rentalService = rentalService;
        }

        [BindProperty]
        public DeliveryPerson DeliveryPerson { get; set; }

        [BindProperty]
        public Identity.Pages.Account.RegisterModel.InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            CnhTypes = CnhHelper.GetAllTypesAsDropdown();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            List<string> Errors = new List<string>();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                await _userManager.AddToRoleAsync(user, "DeliveryPerson");

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    string userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    /*
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    */

                    try
                    {
                        DeliveryPerson.IdentityUserId = userId;

                        DeliveryPerson.CnhPicture ??= await CheckPicture(userId);

                        var deliveryPerson = await _rentalService.AddDeliveryPerson(DeliveryPerson);

                        if (deliveryPerson != null)
                        {
                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("Account/RegisterConfirmation", new { area = "Identity", email = Input.Email, returnUrl = returnUrl });
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                return LocalRedirect(returnUrl);
                            }
                        }

                        throw new Exception("Error adding a new delivery person");

                    }
                    catch (Exception ex)
                    {
                        Errors.Add(ex.Message);
                    }
                    finally
                    {
                        await _userManager.DeleteAsync(user);

                        if (DeliveryPerson.CnhPicture != null)
                        {
                            DeletePicture(DeliveryPerson.CnhPicture);
                            DeliveryPerson.CnhPicture = null;
                        }
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                foreach (var error in Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void DeletePicture(string cnhPicture)
        {
            if (System.IO.File.Exists(cnhPicture))
            {
                System.IO.File.Delete(cnhPicture);
            }
        }

        private async Task<string?> CheckPicture(string newfilename)
        {
            if (CnhPicture == null || CnhPicture.Length == 0)
            {
                return null;
            }

            string fileName = Path.GetFileName(CnhPicture.FileName);
            string ext = Path.GetExtension(CnhPicture.FileName).ToLower();

            if (ext != ".bmp" || ext != ".png")
            {
                throw new Exception("Invalid image format");
            }

            fileName = newfilename + ext;
            string filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await CnhPicture.CopyToAsync(stream);
            }

            return filePath;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    "override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}