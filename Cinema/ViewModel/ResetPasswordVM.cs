namespace Cinema.ViewModel
{
    public class ResetPasswordVM
    {
        public int id {  get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string password { get; set; } =string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string ApplicationID { get; set; } = null!;
    }
}
