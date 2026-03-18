namespace Cinema.Areas.Identity.Controllers
{
    [Area(SD.Area_Identity)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private IRepository<ApplicationOTP> _applicationOTP;
        public AccountController(UserManager<ApplicationUser> usermaneger ,
            SignInManager<ApplicationUser> signInManager ,IEmailSender emailSender,IRepository<ApplicationOTP> applicationOTP) 
        { 
         _userManager = usermaneger;
         _signInManager = signInManager;
         _emailSender = emailSender;
            _applicationOTP = applicationOTP;
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
                TempData["Error"] = "Invalid Data";
                return View(registerVM);
            }
            ApplicationUser applicationuser = new()
            {
                FName = registerVM.FName,
                UserName = registerVM.UserName,
                LName = registerVM.LName,
                Email = registerVM.Email,
                Address = registerVM.Address
            };

            var user = await _userManager.CreateAsync(applicationuser, registerVM.Password);
            if (!user.Succeeded)
            {
                foreach (var item in user.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                TempData["Error"] = "Invalid Data";
                return View(registerVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationuser);
            var ConfigrationEmail = Url.Action("Confirm", "Account", new
            {
                area = "Identity",
                token,
                applicationuser.Id
            }, Request.Scheme);
            await _userManager.AddToRoleAsync(applicationuser, SD.ADMIN_Role);
            await _emailSender.SendEmailAsync(applicationuser.Email, "Confirm Your Email !", $"<h3>Click <a href={ConfigrationEmail}>Here</a> to confirm Your Account<h3>");

            TempData["success-notification"] = "Your Account is Created";
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
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.EmailOrUserName) ??
                     await _userManager.FindByNameAsync(loginVM.EmailOrUserName);
            if (user is null)
            {
                ModelState.AddModelError("EmailORUserName", "Invalid Data");
                ModelState.AddModelError("Password", "Invalid Data");
                return View(loginVM);
            }
            //var result = await _userManager.CheckPasswordAsync(user, loginVM.Password);

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMy, false);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("EmailORUserName", "Confirm Your Email address");
                    return View(loginVM);

                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too Many Attempts, please try again Later");
                    return View(loginVM);

                }
                ModelState.AddModelError("EmailORUserName", "Invalid Data");
                ModelState.AddModelError("Password", "Invalid Data");
                return View(loginVM);
            }
            TempData["Success"] = $"Welcom Back Mester {user.UserName}";
            return RedirectToAction("Defulte", "Home", new { area = "Admin" });
        }
        [HttpGet]
        public IActionResult ResentEmailConfigration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResentEmailConfigration(ResentEmailConfigrationVM resentEmailConfigrationVM)
        {
            if (!ModelState.IsValid) return View(resentEmailConfigrationVM);
            var user = await _userManager.FindByEmailAsync(resentEmailConfigrationVM.Email);
            if(user is null)
            {
                TempData["Error"] = "This Email Is not Valid";
                return View(resentEmailConfigrationVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var ConfigrationEmail = Url.Action("Confirm", "Account", new
            {
                area = "Identity",
                token,
                user.Id
            }, Request.Scheme);
            TempData["Success"] = "Confirm Email Successfully";
            await _emailSender.SendEmailAsync(resentEmailConfigrationVM.Email, "Confirm Your Email !", $"<h3>Click <a href={ConfigrationEmail}>Here</a> to confirm Your Account<h3>");
            return RedirectToAction("Login", "Account", new { area = "Identity", ApplicationID = user.Id });
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid) return View(forgetPasswordVM);
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
            var last24Hours = DateTime.UtcNow.AddHours(-24);
            var userCountOTP = (await _applicationOTP
                .GetAllasync(e => user.Id == e.ApplicationUserID && e.CreatedAt >= last24Hours))
                .Count();
            if (!user.EmailConfirmed) return View(nameof(ResentEmailConfigration));
            else if (user is null)
            {
                TempData["Error"] = "Invalid Email Address";
                ModelState.AddModelError("Email Address", "Invalid Email Address");
                return View(forgetPasswordVM);
            }
            else if (user is not null && userCountOTP <= 3)
            {
                var OTP = new Random().Next(1000, 9999).ToString();
                string msg = $"<h1>This is Your OTP {OTP} Don't Shared this numper</h1>";
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailSender.SendEmailAsync(forgetPasswordVM.Email, "Fogret Password !", msg);
                    await _applicationOTP.CreateAsync(new()
                {
                    ApplicationUserID = user.Id,
                    OTP = OTP,
                });
                await _applicationOTP.CommitAsync();
                TempData["Success"] = "The OTP Confirmation Email Success ,Check Your Email";
            }
            else if (user is not null && userCountOTP > 3)
            {
                TempData["Error"] = "Your Confirmed the last OTP , Try again after 24 Hour";
            }
            else
            {
                TempData["Error"] = "Invalid Email Address";
            }
            return RedirectToAction("ValidOTP", "Account", new { area = "Identity", ApplicationID = user.Id });
        }
        [HttpGet]
        public IActionResult ValidOTP(string ApplicationID)
        {
            return View(new ValidOTPVM
            {
                ApplicationID = ApplicationID
            });
        }
        [HttpPost]
        public async Task<IActionResult> ValidOTP(ValidOTPVM validOTPVM)
        {
            //if (!ModelState.IsValid) return View(validOTPVM);
            var user = await _userManager.FindByIdAsync(validOTPVM.ApplicationID);
            if (user is null) return NotFound();
            var otp = (await _applicationOTP.GetAllasync()).Where(e => e.ApplicationUserID == user.Id && e.IsValied)
                   .OrderBy(e => e.Id).LastOrDefault();
            if (otp == null)
            {
                TempData["Error"] = "Invalid OTP , Please Try again";
                return View(validOTPVM);
            }
            otp.IsUsed = true;
            return RedirectToAction("ResetPassword", "Account", new { area = "Identity", ApplicationID = user.Id });
        }
        [HttpGet]
        public IActionResult ResetPassword(string ApplicationID)
        {
            return View(new ResetPasswordVM
            {
                ApplicationID = ApplicationID
            });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if(!ModelState.IsValid) return View(resetPasswordVM);
            var user = await _userManager.FindByIdAsync(resetPasswordVM.ApplicationID);
            if (user is null) return NotFound();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
           var resulte = (await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.password));
            if(!resulte.Succeeded)
            {
                ModelState.AddModelError("Password", string.Join(", ", resulte.Errors.Select(e => e.Description)));
                return View(resetPasswordVM);
            }
            TempData["Success"] = "Update Password Successed";
            return RedirectToAction("Login", "Account", new { area = "Identity", ApplicationID = user.Id });

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Log out Successed";
            return RedirectToAction(nameof(Login));
        }
    }
}
