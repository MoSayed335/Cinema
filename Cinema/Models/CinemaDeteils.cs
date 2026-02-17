namespace Cinema.Models
{
    public class CinemaDeteils
    {
        [Key]
        public int CinemaId { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public bool Status { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
