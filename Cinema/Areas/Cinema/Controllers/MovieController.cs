using Cinema.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Role_Customer)]
    public class MovieController : Controller
    {
        //private ApplicationDBContxet _db = new();
            private IRepository<Movie> _MovieRepository;
            private IRepository<CinemaDeteils> _CinemaRepository;
        private IRepository<Category> _CategoryRepository;
        private IRepository<Actor> _ActorRepository;

        public MovieController(IRepository<Movie> movieRepository, 
            IRepository<CinemaDeteils> cinemaRepository, IRepository<Category> categoryRepository, IRepository<Actor> actorRepository)
        {
            _MovieRepository = movieRepository;
            _CinemaRepository = cinemaRepository;
            _CategoryRepository = categoryRepository;
            _ActorRepository = actorRepository;
        }

        public async Task<IActionResult> Details(int id)
        {
            //var movie = _db.Movies
            //    .Include(m => m.Cinema)
            //    .Include(m => m.Category)
            //    .Include(m => m.MovieActors)
            //    .ThenInclude(ma => ma.Actor)
            //    .FirstOrDefault(m => m.MovieId == id);
            var movie = await _MovieRepository.GetoneAsync
                (
                    includes: [m => m.Cinema, m => m.Category, m => m.MovieActors],
                    expression: m => m.MovieId == id
                );
            if (movie == null) return NotFound();


            return View(movie);
        }
    }
}
