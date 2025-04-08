using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Models;
using MobileShopInMVC.Services;

namespace MobileShopInMVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly RazorpayService _razorpayService;
        private readonly ApplicationDbContext _context;

        public CheckoutController(RazorpayService razorpayService, ApplicationDbContext context)
        {
            _razorpayService = razorpayService;
            _context = context;
        }

        public IActionResult Index(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            string razorpayOrderId = _razorpayService.CreateOrder(order.TotalAmount);

            ViewBag.OrderId = orderId;
            ViewBag.RazorpayOrderId = razorpayOrderId;
            ViewBag.Amount = order.TotalAmount * 100;
            ViewBag.Key = _razorpayService.GetKeyId(); // Get Razorpay Key ID

            return View();
        }
        public IActionResult PaymentSuccess(int paymentId, int orderId )
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            order.Status = "Paid";

            _ = _context.Payments.Add(new Payment
            {
                OrderId = orderId,
                PaymentId = paymentId,
                Amount = order.TotalAmount,
                //PaymentStatus = "Completed",  // ✅ Fixed
                PaymentDate = DateTime.Now
            });

            _context.SaveChanges();

            return RedirectToAction("Success");
        }


        public IActionResult Success()
        {
            return View();
        }

    }
}
