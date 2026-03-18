using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    [Authorize(Roles = $"{SD.ADMIN_Role},{SD.SuperAdmin_Role},{SD.Employee_Role}")]

    public class MoviesController : Controller
    {
        //private ApplicationDBContxet _db = new();
        private IRepository<Movie> _MovieRepository;
        private IRepository<CinemaDeteils> _CinemaRepository;
        private IRepository<Category> _CategoryRepository;

        public MoviesController(IRepository<Movie> movieRepository,
            IRepository<CinemaDeteils> cinemaRepository, IRepository<Category> categoryRepository)
        {
            _MovieRepository = movieRepository;
            _CinemaRepository = cinemaRepository;
            _CategoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var Movies = _db.Movies.AsNoTracking().AsQueryable();
            //var categores = _db.Categories.AsNoTracking().AsQueryable();
            //var cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            var categores = await _CategoryRepository.GetAllasync(Tracke: false);
            var cinemas = await _CinemaRepository.GetAllasync(Tracke: false);    
            //Movies = _db.Movies
            //    .Include(m => m.Category)
            //    .Include(m => m.Cinema);
            var Movies = await _MovieRepository.GetAllasync(includes: [m => m.Category, m => m.Cinema], Tracke: false);
            return View(Movies);
        }

        [HttpGet]
        public async Task<IActionResult> create()
        {
            //var Categories = _db.Categories.AsNoTracking().AsQueryable();
            // var Cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            var Categories = await _CategoryRepository.GetAllasync();
            var Cinemas = await _CinemaRepository.GetAllasync();
            var vm = new MoviesCreateVM
            {
                Categories = Categories.ToList(),
                Cinemas = Cinemas.ToList()
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie Move, IFormFile MainImg)
        {
            //if (!ModelState.IsValid)
            //    var model = new MoviesCreateVM
            //    {
            //        Categories = Categories.AsEnumerable(),
            //        Cinemas = Cinemas.AsEnumerable(),
            //        Movie = movies.Movie
            //    };
            //return View(model);
            //model.Cinemas = _db.Cinemas.AsNoTracking().ToList();  
            //model.Categories = _db.Categories.AsNoTracking().ToList();
            var Categories = await _CategoryRepository.GetAllasync();
            var Cinemas = await _CinemaRepository.GetAllasync();
            if (MainImg is not null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Movie", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    MainImg.CopyTo(stream);
                }
                Move.MainImg = fileName;
            }
            //if (movies.Movie.CategoryId == 0 || movies.Movie.CinemaId == 0)
            //{
            //    //var model = new MoviesCreateVM
            //    //{
            //    //    Categories = _db.Categories.ToList(),
            //    //    Cinemas = _db.Cinemas.ToList(),
            //    //    Movie = movie
            //    //};
            //    var model = new MoviesCreateVM
            //    {
            //        Categories = Categories.AsEnumerable(),
            //        Cinemas = Cinemas.AsEnumerable(),
            //        Movie = movies.Movie
            //    };
            //    return View(model);
            //}
            //_db.Movies.Add(movie);
            //_db.SaveChanges();
            TempData["Success"] = "Movie created successfully";

            await _MovieRepository.CreateAsync(Move);
            await _MovieRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var movieFromDb = await _MovieRepository.GetoneAsync(m => m.MovieId == id);
            if (id is null || id == 0) return NotFound();
            //var movieFromDb = _db.Movies.Find(id);
            if (movieFromDb == null) return NotFound();
            //var Categories = _db.Categories.AsNoTracking().AsQueryable();
            //var Cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            var Categories = await _CategoryRepository.GetAllasync();
            var Cinemas = await _CinemaRepository.GetAllasync();
            var model = new MoviesEditVM
            {
                Movie = movieFromDb,
                Categories = Categories.ToList(),
                Cinemas = Cinemas.ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Movie movie, IFormFile? MainImg)
        {
            var movieFromDb = await _MovieRepository.GetoneAsync(expression: m => m.MovieId == movie.MovieId);
            if (movieFromDb == null) return NotFound();

            // تحديث الصورة
            if (MainImg != null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Movie", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await MainImg.CopyToAsync(stream);
                }

                var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Movie", movieFromDb.MainImg);
                if (System.IO.File.Exists(oldImgPath))
                    System.IO.File.Delete(oldImgPath);

                movieFromDb.MainImg = fileName;
            }

            // تحديث باقي الخصائص
            movieFromDb.Name = movie.Name;
            movieFromDb.Price = movie.Price;
            movieFromDb.Description = movie.Description;
            movieFromDb.CinemaId = movie.CinemaId;
            movieFromDb.CategoryId = movie.CategoryId;
            movieFromDb.DateTime = movie.DateTime;
            movieFromDb.Status = movie.Status;
            TempData["Success"] = "Movie Edit successfully";

            _MovieRepository.ubdate(movie);
            await _MovieRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var movieFromDb = await _MovieRepository.GetoneAsync(m => m.MovieId == id);
            if (id is null || id == 0) return NotFound();
            //var movieFromDb = _db.Movies.Find(id);
            if (movieFromDb == null) return NotFound();
            var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Movie", movieFromDb.MainImg);
            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }

            //_db.Movies.Remove(movieFromDb);
            //_db.SaveChanges();
            TempData["Success"] = "Movie Delete successfully";

            _MovieRepository.Delete(movieFromDb);
            await _MovieRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
