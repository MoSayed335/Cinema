namespace Cinema.ViewModel
{
    public class RegisterVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress] 
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required]

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }

    }
}
