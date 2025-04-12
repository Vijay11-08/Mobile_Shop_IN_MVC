using Microsoft.AspNetCore.Mvc;

namespace MobileShopInMVC.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
