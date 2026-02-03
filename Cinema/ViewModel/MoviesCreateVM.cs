namespace Cinema.ViewModel
{
    public class MoviesCreateVM
    {
            public Movie Movie { get; set; }
            public IEnumerable<CinemaDeteils> Cinemas { get; set; }
            public IEnumerable<Category> Categories { get; set; }

    }
}
