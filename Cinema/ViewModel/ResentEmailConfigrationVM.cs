namespace Cinema.ViewModel
{
    public class ResentEmailConfigrationVM
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
