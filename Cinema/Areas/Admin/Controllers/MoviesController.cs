using Cinema.DataAccess;
using Cinema.Models;
using Cinema.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Cinema.Areas.Admin.Controllers
{
   [Area(SD.Role_Admin)]
    public class MoviesController : Controller
    {
        private ApplicationDBContxet _db = new();
        public IActionResult Index()
        {
            var Movies = _db.Movies.AsNoTracking().AsQueryable();
            //var categores = _db.Categories.AsNoTracking().AsQueryable();
            //var cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            Movies = _db.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema);
            return View(Movies);
        }

        [HttpGet]
        public IActionResult create()
        {
           var Categories = _db.Categories.AsNoTracking().AsQueryable();
            var Cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            
            return View(new MoviesCreateVM
            {
                Categories = Categories.AsEnumerable(),
                Cinemas = Cinemas.AsEnumerable()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie , IFormFile MainImg)
        {
            if (!ModelState.IsValid) return View(movie);
            //model.Cinemas = _db.Cinemas.AsNoTracking().ToList();  
            //model.Categories = _db.Categories.AsNoTracking().ToList();
            if (movie.CategoryId == 0 || movie.CinemaId == 0)
            {
                var model = new MoviesCreateVM
                {
                    Categories = _db.Categories.ToList(),
                    Cinemas = _db.Cinemas.ToList(),
                    Movie = movie
                };
                return View(model);
            }
            if (MainImg is not null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Cinema", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    MainImg.CopyTo(stream);
                }
                movie.MainImg = fileName;
            }
            _db.Movies.Add(movie);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
