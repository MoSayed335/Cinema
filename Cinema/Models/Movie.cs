namespace Cinema.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime DateTime { get; set; }
        public string MainImg { get; set; }
        public int CinemaId { get; set; }
        public int CategoryId { get; set; }
        public CinemaDeteils Cinema { get; set; }
        public Category Category { get; set; }
        public List<MovieActor> MovieActors { get; set; }
    }
}
