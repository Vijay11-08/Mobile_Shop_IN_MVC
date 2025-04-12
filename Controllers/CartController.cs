using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Helpers;
using MobileShopInMVC.Models;
using System.Collections.Generic;
using System.Linq;
using MobileShopInMVC.Data;

namespace MobileShopInMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Get Cart Items
        public IActionResult Index()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // Add Product to Cart
        public IActionResult AddToCart(int id)
        {
            var product = _context.Product.Find(id);
            if (product == null)
                return NotFound();

            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(p => p.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price
                });
            }

            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        // Remove Item from Cart
        public IActionResult RemoveFromCart(int id)
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null)
                return RedirectToAction("Index");

            var itemToRemove = cart.FirstOrDefault(p => p.ProductId == id);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // Checkout Cart
        public IActionResult Checkout()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
                return RedirectToAction("Index");

            return View(cart);
        }

        // Handle Payment Success
        public IActionResult PaymentSuccess(string paymentId)
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (cart != null)
            {
                foreach (var item in cart)
                {
                    var order = new Orders
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        BuyerName = User.Identity.Name,
                        Email = User.Identity.Name,
                        Price = item.Price,
                        PaymentId = paymentId
                    };
                    _context.Orders.Add(order);
                }

                _context.SaveChanges();
                _httpContextAccessor.HttpContext.Session.Remove("Cart");
            }

            return View();
        }
    }
}
