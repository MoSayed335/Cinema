namespace Cinema.ViewModel
{
    public class LoginVM
    {
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        [Display (Name ="Email Or User_Name")]
        public string EmailOrUserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } =string.Empty;
        public bool RememberMy {  get; set; }
    }
}
