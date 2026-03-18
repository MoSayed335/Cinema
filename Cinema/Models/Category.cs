namespace Cinema.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
            [Required, MaxLength(100)]
            public string Name { get; set; }
            public bool Status { get; set; }
            public List<Movie> Movies { get; set; }
 
    }
}
