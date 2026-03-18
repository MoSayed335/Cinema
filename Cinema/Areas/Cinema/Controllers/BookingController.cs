using Microsoft.AspNetCore.Authorization;
using Stripe.Checkout;

namespace Cinema.Areas.Cinema.Controllers
{
    [Area(SD.Area_Customer)]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Movie> _movieRepository;

        public BookingController(IRepository<Movie> movieRepository, IRepository<Booking> bookingRepository, UserManager<ApplicationUser> userManager)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _movieRepository = movieRepository;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _bookingRepository.GetAllasync(b => b.UserId == user!.Id, includes: [b => b.Movie]);
            return View(bookings);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var movies = await _movieRepository.GetAllasync();

            return View(new BookingVM()
            {
                Movies = movies.ToList(),
                ShowTime = DateTime.Now.AddDays(1)
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingVM bookingVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["Error"] = "Please correct the errors in the form.";
            //    return View(bookingVM);
            //}
            var user = await _userManager.GetUserAsync(User);
            decimal pricePerTicket = bookingVM.SeatType == SeatType.VIP ? 150 : 100;
            decimal totalPrice = pricePerTicket * bookingVM.NumbetOFTikets;
            var booking = new Booking
            {
                UserId = user!.Id,
                MovieId = bookingVM.MovieId,
                SeatType = bookingVM.SeatType,
                NumbetOFTikets = bookingVM.NumbetOFTikets,
                ShowTime = bookingVM.ShowTime,
                BookingDate = DateTime.UtcNow,
                TotalPrice = totalPrice
            };
            await _bookingRepository.CreateAsync(booking);
            await _bookingRepository.CommitAsync();
            TempData["Success"] = "Booking created successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detalis([FromRoute] int id)
        {
            var booking = await _bookingRepository.GetoneAsync(b => b.Id == id, includes: [b => b.Movie, b => b.User]);
            if (booking == null) return NotFound();
            return View(booking);
        }
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var booking = await _bookingRepository.GetoneAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            _bookingRepository.Delete(booking);
            await _bookingRepository.CommitAsync();
            TempData["Success"] = "Booking deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Payment()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };
            var carts = await _bookingRepository.GetoneAsync(c => c.UserId == user.Id, includes: [c => c.Movie]);
            if (carts is not null)
            {
                //var Code = await _cartRepository.GetoneAsync(c => c.ApplicationUserId == user.Id);
                //foreach (var item in carts)
                //{
                    //var promotion = (await _promotionRepository.GetAllasync(p => p.Code == Code));
                    //var Discount = promotion.Where(p => p.ProductId == item.ProductId).Select(p => p.Discount).FirstOrDefault();
                    //item.Price = item.Product.Price * (1 - item.Product.Discount + Discount / 100);
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = carts.Movie.Name,
                                Description = carts.Movie.Description,
                            },
                            UnitAmount = (long)carts.TotalPrice * 100,
                        },
                        Quantity = carts.NumbetOFTikets,
                    });
                
            }
            var service = new SessionService();
            var session = service.Create(options);
            TempData["success-notification"] = "Pay Is Succcefully";
            return Redirect(session.Url);
        }
    }
}
