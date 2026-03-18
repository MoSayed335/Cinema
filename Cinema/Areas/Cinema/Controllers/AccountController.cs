using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Threading.Tasks;

namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Area_Customer)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill the form correctly.";
                return View(registerVM);
            }
            ApplicationUser user = new()
            {
                FName = registerVM.FName,
                LName = registerVM.LName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Address = registerVM.Address
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.");
            await _userManager.AddToRoleAsync(user, SD.Customer_Role);
            TempData["Success"] = "Registration successful! Please check your email to confirm your account.";
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Confirm(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return View(user);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    TempData["error-notification"] = "Invalid Email Configration";
                }
            }
            else
            {
                TempData["success-notification"] = "Your Email Is Confirmed";
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill the form correctly.";
                return View(loginVM);
            }
            var user = await _userManager.FindByEmailAsync(loginVM.EmailOrUserName);

            if (user is null)
            {
                ModelState.AddModelError("EmailOrUserName", "Invalid Data.");
                ModelState.AddModelError("Password", "Invalid Data.");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMy, true);
            if(!result.Succeeded)
            {
               if(result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked. Please try again later.");
                    return View(loginVM);
                }
               if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("EmailOrUserName", "Please confirm your email before logging in.");
                    return View(loginVM);
                }
                ModelState.AddModelError("EmailOrUserName", "Invalid Data.");
                ModelState.AddModelError("Password", "Invalid Data.");
                return View(loginVM);
            }
            TempData["Success"] = "Login successful!";

            return RedirectToAction("Index", "Home", new { area = "Cinema" });
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
                TempData["Success"] = "Logout successful!";
            return RedirectToAction(nameof(Login));

        }

    }
}
