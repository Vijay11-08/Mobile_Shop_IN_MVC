using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Models;

namespace MobileShopInMVC.Controllers
{
    public class CartController : Controller
    {
        private static List<CartItem> Cart = new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItem item)
        {
            Cart.Add(item);
            return Json(new { message = "Product added to cart!" });
        }
    }
}
