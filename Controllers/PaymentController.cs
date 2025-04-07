using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace MobileShopInMVC.Controllers
{
    public class PaymentController : Controller
    {
        private const string Key = "rzp_test_x8tV5oSUixLmbV";
        private const string Secret = "fWH4faC9rEJ9StONJyc8ZXCc";

        public IActionResult CreateOrder(decimal amount)
        {
            RazorpayClient client = new RazorpayClient(Key, Secret);
            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amount * 100 }, // Amount in paise
            { "currency", "INR" },
            { "receipt", "order_rcptid_11" }
        };
            Order order = client.Order.Create(options);
            return Json(new { orderId = order["id"] });
        }
    }
}
