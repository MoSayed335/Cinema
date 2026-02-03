using Cinema.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Role_Admin)]
    public class CinemaController : Controller
    {
        private ApplicationDBContxet _db = new();
        public IActionResult Index()
        {
            var cinemas = _db.Cinemas.AsNoTracking().AsQueryable();
            return View(cinemas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CinemaDeteils cinemaDeteils, IFormFile Image)
        {
            //if (!ModelState.IsValid) return View(cinemaDeteils);

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

            _db.Cinemas.Add(cinemaDeteils);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();
            
            var cinemaFromDb = _db.Cinemas.Find(id);
            if (cinemaFromDb == null) return NotFound();
            return View(cinemaFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CinemaDeteils cinemaDeteils, IFormFile? Image)
        {
            //if (!ModelState.IsValid) return View(cinemaDeteils);
            var cinemaFromDb = _db.Cinemas.AsNoTracking().FirstOrDefault(c => c.CinemaId == cinemaDeteils.CinemaId);
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
            _db.Cinemas.Update(cinemaDeteils);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var cinemaFromDb = _db.Cinemas.Find(id);
            if (cinemaFromDb == null) return NotFound();
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Cinema", cinemaFromDb.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _db.Cinemas.Remove(cinemaFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
