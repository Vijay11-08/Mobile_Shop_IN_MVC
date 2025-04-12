using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Models;
using System;
using System.Threading.Tasks;
using MobileShopInMVC.Data;

namespace MobileShopInMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Success(string paymentId, int productId, decimal price)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
                return NotFound();

            var order = new Orders
            {
                ProductId = productId,
                BuyerName = "John Doe", // Hardcoded for now, you can add a form for user details
                Email = "johndoe@example.com",
                Price = price,
                PaymentId = paymentId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return View(order);
        }
    }
}
