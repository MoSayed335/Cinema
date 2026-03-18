using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    [Authorize(Roles = $"{SD.ADMIN_Role},{SD.SuperAdmin_Role},{SD.Employee_Role}")]

    public class ActorController : Controller
    {
        private readonly IRepository<Actor> _actor;

        public ActorController(IRepository<Actor> Actor)
        {
            _actor = Actor;
        }

        public async Task<IActionResult> Index(string? name, int page = 1)
        {
            int pageSize = 5;

            var actors = await _actor.GetAllasync(includes: [e=>e.MovieActors],Tracke:false);
            var totalActors = actors.Count();

            // Search
            if (!string.IsNullOrEmpty(name))
            {
                actors = actors.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = actors.Count();
            double totalPages = Math.Ceiling(totalCount / (double)pageSize);

            var data = actors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(new ActorVM
            {
                Actors = data,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchName = name
            });
        }

        // =========================
        // GET: Create
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // POST: Create
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Actor actor, IFormFile Image)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["Error"] = "Please correct the errors in the form.";
            //    return View(actor);
            //}

                if (Image != null && Image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Actors", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    actor.Image = fileName;
            }


            await _actor.CreateAsync(actor);
            await _actor.CommitAsync();
            TempData["Success"] = "Actor created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // GET: Edit
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _actor.GetoneAsync(a => a.ActorId == id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        // =========================
        // POST: Edit
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Actor actor, IFormFile? image)
        {
            //if (!ModelState.IsValid)
            //    return View(actor);
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/actors", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                actor.Image = fileName;
            } else {
                var existingActor = await _actor.GetoneAsync(a => a.ActorId == actor.ActorId);
                if (existingActor != null)
                {
                    actor.Image = existingActor.Image;
                }
            }
            _actor.ubdate(actor);
            await _actor.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // Delete
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actor.GetoneAsync(a => a.ActorId == id);

            if (actor == null)
                return NotFound();
            if (!string.IsNullOrEmpty(actor.Image))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/actors", actor.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _actor.Delete(actor);
            await _actor.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
