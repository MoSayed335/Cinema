using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace Cinema.Services.DbIntialize
{
    public class Dbintialize : IDbintialize
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContxet _context;

        public Dbintialize(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> usermanager, ApplicationDBContxet context)
        {
            _roleManager = roleManager;
            _userManager = usermanager;
            _context = context;
        }
        public async Task Intializer()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new(SD.SuperAdmin_Role));
                await _roleManager.CreateAsync(new(SD.ADMIN_Role));
                await _roleManager.CreateAsync(new(SD.Employee_Role));
                await _roleManager.CreateAsync(new(SD.Customer_Role));

                await _userManager.CreateAsync(new()
                {
                    FName = "Super",
                    LName = "Admin",
                    Email = "SuperAdmin@gmail.com",
                    EmailConfirmed = true,
                    UserName = "SuperAdmin"
                }, "Super123@");
                await _userManager.CreateAsync(new()
                {
                    FName = "Admin",
                    LName = "1",
                    Email = "Admin@gmail.com",
                    EmailConfirmed = true,
                    UserName = "Admin"
                }, "Admin123@");
                await _userManager.CreateAsync(new()
                {
                    FName = "Employee",
                    LName = "1",
                    Email = "Employee@gmail.com",
                    EmailConfirmed = true,
                    UserName = "Employee"
                }, "Employee123@");
                var user = await _userManager.FindByNameAsync("SuperAdmin");
                var user2 = await _userManager.FindByNameAsync("Admin");
                var user3 = await _userManager.FindByNameAsync("Employee");
                if (user is not null && user2 is not null && user3 is not null)
                {
                    await _userManager.AddToRoleAsync(user, SD.SuperAdmin_Role);
                    await _userManager.AddToRoleAsync(user2, SD.ADMIN_Role);
                    await _userManager.AddToRoleAsync(user3, SD.Employee_Role);
                }
            }

        }
    }
}
