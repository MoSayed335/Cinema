using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    [Authorize(Roles =$"{SD.ADMIN_Role},{SD.SuperAdmin_Role},{SD.Employee_Role}")]
    public class HomeController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;

        public IRepository<CinemaDeteils> _CinemaRepository;
        public IRepository<Booking> _bookingRepository; 

        public HomeController(IRepository<Movie> movieRepository, IRepository<Booking> bookingRepository ,IRepository<CinemaDeteils> cinemaRepository)
        {
            _movieRepository = movieRepository;
            _CinemaRepository = cinemaRepository;
            _bookingRepository = bookingRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Defulte()
        {
            var vm = new DashboardVM();

            // Total Counts
            vm.TotalMovies = (await _movieRepository.GetAllasync()).Count();
            vm.TotalCinemas = (await _CinemaRepository.GetAllasync(Tracke: false)).Count();
            vm.TotalBookings = (await _bookingRepository.GetAllasync()).Count();
            // Most Recent Movie
            vm.MostRecentMovie = (await _movieRepository.GetAllasync())
                .OrderByDescending(m => m.MovieId)
                .Select(m => m.Name)
                .FirstOrDefault();
            var Movies = await _movieRepository.GetAllasync(includes: [e => e.Cinema]);
            vm.Movies = Movies;
            // Latest 5 Movies
            vm.LatestMovies = (await _movieRepository.GetAllasync())
                .OrderBy(m => m.MovieId)
                .Take(5)
                .Select(m => m.Name)
                .ToList();



            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Manegment()
        {
            var cinemas = await _CinemaRepository.GetAllasync();
            return View(cinemas);
        }
        [HttpGet]

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var cinema = await _CinemaRepository.GetoneAsync(expression: c => c.CinemaId == id, includes: [c =>c.Movies]);
            if (cinema == null) return NotFound();
            return View(cinema);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(CinemaDeteils cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            var cinemaFromDb = await _CinemaRepository.GetoneAsync(expression: c => c.CinemaId == cinema.CinemaId);
            if (cinemaFromDb == null) return NotFound();
            cinemaFromDb.Name = cinema.Name;
            cinemaFromDb.Location = cinema.Location;
            _CinemaRepository.ubdate(cinemaFromDb);
            await _CinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Manegment));
        }
    }
}
