using Cinema.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Role_Customer)]
    public class MovieController : Controller
    {
        private ApplicationDBContxet _db = new();
        public IActionResult Details(int id)
        {
            var movie = _db.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Category)
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null) return NotFound();

            return View(movie);
        }
    }
}
