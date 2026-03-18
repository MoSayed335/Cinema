namespace Cinema.Models
{
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }
        [Required, MaxLength(120)]
        public string Name { get; set; }
        public string Image { get; set; }
        public List<MovieActor> MovieActors { get; set; }
    }
}
