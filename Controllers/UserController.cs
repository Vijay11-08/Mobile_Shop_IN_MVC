using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileShopInMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BuyIphone()
        {
            return View();
        }

        public IActionResult BuySamsung()
        {
            return View();
        }
        public IActionResult BuyAndroid()
        {
            return View();
        }
        public IActionResult BuyIpad()
        {
            return View();
        }
        public IActionResult BuySmartwatch()
        {
            return View();
        }
        public IActionResult BuyMacbook()
        {
            return View();
        }
        public IActionResult BuyAccessroies()
        {
            return View();
        }
        public IActionResult Clearance()
        {
            return View();
        }
        public IActionResult SellIphone()
        {
            return View();
        }
        public IActionResult SellSamsung()
        {
            return View();
        }
        public IActionResult SellAndroid()
        {
            return View();
        }
        public IActionResult SellSmartwatch()
        {
            return View();
        }
        public IActionResult SellIpad()
        {
            return View();
        }
        public IActionResult SellOtheritem()
        {
            return View();
        }
    }
}
