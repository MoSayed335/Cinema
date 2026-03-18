using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    [Authorize(Roles = $"{SD.ADMIN_Role},{SD.Employee_Role},{SD.SuperAdmin_Role}")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            if (users == null || users.Count == 0)
            {
                return View(new List<UserWithRoleVM>());
            }

            var userList = new List<UserWithRoleVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserWithRoleVM
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return View(userList);
        }
    }
}