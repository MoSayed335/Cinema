namespace Cinema.ViewModel
{
    public class ValidOTPVM
    {
        public int id { get; set; }
        public string OTP { get; set; } = string.Empty;
        public string ApplicationID { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
