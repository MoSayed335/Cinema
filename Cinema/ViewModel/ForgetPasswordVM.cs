namespace Cinema.ViewModel
{
    public class ForgetPasswordVM
    {
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        [Display (Name ="Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
