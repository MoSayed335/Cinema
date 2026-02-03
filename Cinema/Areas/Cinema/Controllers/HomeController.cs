using Cinema.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Role_Customer)]
    public class HomeController : Controller
    {
        private ApplicationDBContxet _db = new();
        public IActionResult Index()
        {
            var Cinemas = _db.Cinemas
                .Select(c => new CinemaIndexVM
                {
                    CinemaId = c.CinemaId,
                    Name = c.Name,
                    Image = c.Image
                }).ToList();

            return View(Cinemas);
        }
            public IActionResult Details(int id)
        {
            var cinema = _db.Cinemas
                .Include(c => c.Movies)
                .FirstOrDefault(c => c.CinemaId == id);

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
