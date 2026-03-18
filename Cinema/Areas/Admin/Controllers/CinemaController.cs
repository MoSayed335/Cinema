using Microsoft.AspNetCore.Authorization;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    [Authorize(Roles = $"{SD.ADMIN_Role},{SD.SuperAdmin_Role},{SD.Employee_Role}")]

    public class CinemaController : Controller
    {
        //private ApplicationDBContxet _db = new();
        //private Repository<CinemaDeteils> _CinemaRepository = new Repository<CinemaDeteils>();
        private IRepository<CinemaDeteils> _CinemaRepository;
        public CinemaController(IRepository<CinemaDeteils> CinemaRepository)
        {
            _CinemaRepository = CinemaRepository;
        }
        public async Task<IActionResult> Index(CinemaDeteils cinema)
        {
            //var cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            var cinemas = await _CinemaRepository.GetAllasync(Tracke: false);
            if(cinema.Name is not null)
                cinemas = await _CinemaRepository.GetAllasync(e=>e.Name.Contains(cinema.Name));

            return View(cinemas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CinemaDeteils cinemaDeteils, IFormFile Image)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["error"] = "Please fill all required fields";
            //    return View(cinemaDeteils);
            //}

            if (Image != null && Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Cinema", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                cinemaDeteils.Image = fileName;
            }

            //_db.Cinemas.Add(cinemaDeteils);
            //_db.SaveChanges();
            await _CinemaRepository.CreateAsync(cinemaDeteils);
            await _CinemaRepository.CommitAsync();
            TempData["success"] = "Cinema created successfully";
            return RedirectToAction(nameof(Index));
        
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();
            
            //var cinemaFromDb = _db.Cinemas.Find(id);
            var cinemaFromDb = await _CinemaRepository.GetoneAsync(c => c.CinemaId == id);
            if (cinemaFromDb == null) return NotFound();
            return View(cinemaFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CinemaDeteils cinemaDeteils, IFormFile? Image)
        {
            //if (!ModelState.IsValid) return View(cinemaDeteils);
            //var cinemaFromDb = _db.Cinemas.AsNoTracking().FirstOrDefault(c => c.CinemaId == cinemaDeteils.CinemaId);
                var cinemaFromDb = await _CinemaRepository.GetoneAsync(c => c.CinemaId == cinemaDeteils.CinemaId, Tracke: false);
            if (cinemaFromDb == null) return NotFound();
            if (Image != null && Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Cinema", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Cinema", cinemaFromDb.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                cinemaDeteils.Image = fileName;
            }
            else
            {
                cinemaDeteils.Image = cinemaFromDb.Image;
            }
            //_db.Cinemas.Update(cinemaDeteils);
            //_db.SaveChanges();
            TempData["success"] = "Cinema updated successfully";
            _CinemaRepository.ubdate(cinemaDeteils);
            await _CinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            //var cinemaFromDb = _db.Cinemas.Find(id);
            var cinemaFromDb = await _CinemaRepository.GetoneAsync(c => c.CinemaId == id);
            if (cinemaFromDb == null) return NotFound();
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Cinema", cinemaFromDb.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            //_db.Cinemas.Remove(cinemaFromDb);
            //_db.SaveChanges();
            TempData["success"] = "Cinema deleted successfully";
            _CinemaRepository.Delete(cinemaFromDb);
            await _CinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
