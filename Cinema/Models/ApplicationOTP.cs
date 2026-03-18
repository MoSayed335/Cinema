namespace Cinema.Models
{
    public class ApplicationOTP
    {
        public int Id { get; set; }
        public string OTP { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; }
        public DateTime Exp { get; set; } = DateTime.UtcNow.AddHours(3);
        public bool IsValied => Exp > DateTime.UtcNow && IsUsed == false;
        public string ApplicationUserID { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
