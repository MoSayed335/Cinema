namespace Cinema.ViewModel
{
    public class MoviesEditVM
    {
        public Movie Movie { get; set; }
        public IEnumerable<CinemaDeteils> Cinemas { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
