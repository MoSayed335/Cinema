using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    [Authorize(Roles = $"{SD.ADMIN_Role},{SD.SuperAdmin_Role},{SD.Employee_Role}")]
    public class BookingController : Controller
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public BookingController(IRepository<Booking> bookingRepository,UserManager<ApplicationUser> userManager)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepository.GetAllasync(includes: [b => b.Movie , b=>b.User]);
            return View(bookings);
        }

        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var booking = await _bookingRepository.GetoneAsync(e => e.Id == id);
            if (booking == null) return NotFound();
            _bookingRepository.Delete(booking);
             await _bookingRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _bookingRepository.GetoneAsync(e=>e.Id ==id);

            if (booking != null)
            {
                _bookingRepository.Delete(booking);
                await _bookingRepository.CommitAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
