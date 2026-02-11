namespace Cinema.Areas.Admin.Controllers
{
    [Area(SD.Role_Admin)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
