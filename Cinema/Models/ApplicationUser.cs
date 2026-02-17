using Microsoft.AspNetCore.Identity;
namespace Cinema.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FName {  get; set; } = string.Empty;
        public string LName { get; set; } =string.Empty;
        public string? Address { get; set; } = string.Empty;
    }
}
