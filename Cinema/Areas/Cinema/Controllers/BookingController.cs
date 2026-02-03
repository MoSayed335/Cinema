using Cinema.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Role_Customer)]
    public class BookingController : Controller
    {
        private ApplicationDBContxet _context = new();
        public IActionResult SelectSeat(int movieId)
        {
            var movie = _context.Movies
                .Include(m => m.Cinema)
                .ThenInclude(c => c.Seats)
                .FirstOrDefault(m => m.MovieId == movieId);

            if (movie == null) return NotFound();

            return View(movie);
        }

        // Step 2: Confirm Booking
        //[HttpPost]
        //public IActionResult Confirm(int movieId, int seatId)
        //{
        //    var seat = _context.Seats.AsNoTracking().AsQueryable();
        //    if (seat.IsBooked) return BadRequest("Seat already booked");

           
        //    _context.Tickets.Add(ticket);
        //    _context.SaveChanges();

        //    return RedirectToAction("Ticket", new { id = ticket.TicketId});
        //}

        // Step 3: Ticket
        public IActionResult Ticket(int id)
        {
            var ticket = _context.Tickets
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .FirstOrDefault(t => t.TicketId == id);

            return View(ticket);
        }
    }
}
