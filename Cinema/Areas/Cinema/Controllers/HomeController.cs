namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Role_Customer)]
    public class HomeController : Controller
    {
        //private ApplicationDBContxet _db = new();
        private IRepository<CinemaDeteils> _CinemaRepository;
        private IRepository<Movie> _MovieRepository;

        public HomeController(IRepository<CinemaDeteils> cinemaRepository, IRepository<Movie> movieRepository)
        {
            _CinemaRepository = cinemaRepository;
            _MovieRepository = movieRepository;
        }

        public async Task<IActionResult> Index()
        {
      
            var Cinemas = await _CinemaRepository.GetAllasync();
            return View(Cinemas);
        }
            public async Task<IActionResult> Details(int id)
        {
            //var cinema = _db.Cinemas
            //    .Include(c => c.Movies)
            //    .FirstOrDefault(c => c.CinemaId == id);
            var cinema = await _CinemaRepository.GetoneAsync(includes: [c => c.Movies], expression: c => c.CinemaId == id);

            if (cinema == null) return NotFound();

            var vm = new CinemaMoviesVM
            {
                CinemaName = cinema.Name,
                Movies = cinema.Movies
                    .Where(m => m.Status)
                    .ToList()
            };

            return View(vm);
        }
    }
}
