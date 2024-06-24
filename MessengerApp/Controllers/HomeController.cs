using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Chat");
            }
            return View();
        }
    }
}
