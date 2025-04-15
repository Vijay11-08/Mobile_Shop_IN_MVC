using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using MobileShopInMVC.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MobileShopInMVC.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Stripe.Checkout;
using Stripe;
using Microsoft.Extensions.Hosting;
using LocalProduct = MobileShopInMVC.Models.Product;
using Newtonsoft.Json;
using System.Linq;

namespace MobileShopInMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        private readonly IConfiguration _configuration;

        public ProductController(IWebHostEnvironment hostEnvironment, IConfiguration configuration = null)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            if (_configuration != null)
            {
                StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            }
        }

        // GET: Product
        [HttpGet]
        public IActionResult Index()
        {

            List<LocalProduct> products = new();
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM Product", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new LocalProduct
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Photo = reader["Photo"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"])
                    });
                }
            }
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocalProduct product)
        {
            string uniqueFileName = null;

            if (product.PhotoFile != null)
            {
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.PhotoFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.PhotoFile.CopyTo(fileStream);
                }
            }

            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("INSERT INTO Product (Name, Photo, Description, Price) VALUES (@Name, @Photo, @Description, @Price)", conn);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Photo", uniqueFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            LocalProduct product = new();
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM Product WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product.Id = id;
                    product.Name = reader["Name"].ToString();
                    product.Photo = reader["Photo"].ToString();
                    product.Description = reader["Description"].ToString();
                    product.Price = Convert.ToDecimal(reader["Price"]);
                }
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LocalProduct product)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("UPDATE Product SET Name=@Name, Description=@Description, Price=@Price WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("DELETE FROM Product WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Buy/5
        public IActionResult Buy(int id)
        {
            BuyerViewModel viewModel = new();

            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM Product WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    viewModel.ProductId = id;
                    viewModel.ProductName = reader["Name"].ToString();
                    viewModel.ProductPhoto = reader["Photo"].ToString();
                    viewModel.ProductPrice = Convert.ToDecimal(reader["Price"]);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(BuyerViewModel model)
        {
            return View("Confirm", model);
        }

        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return cart == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        public IActionResult BuyNow(int id)
        {
            var cart = GetCart();

            var existing = cart.FirstOrDefault(c => c.ProductId == id);
            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                using (SqlConnection conn = new(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new("SELECT * FROM Product WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cart.Add(new CartItem
                        {
                            ProductId = id,
                            ProductName = reader["Name"].ToString(),
                            Photo = reader["Photo"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Quantity = 1
                        });
                    }
                }
            }

            SaveCart(cart);
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var cart = GetCart();
            var total = cart.Sum(item => item.Quantity * item.Price);
            ViewData["Total"] = total;
            return View(cart);
        }

        // POST: Product/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout()
        {
            var stripePublishableKey = _configuration["Stripe:PublishableKey"];
            ViewData["StripePublishableKey"] = stripePublishableKey;

            var cart = GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction("Cart"); // If the cart is empty, redirect to Cart page
            }

            var domain = "https://localhost:7192/Product"; // Change to your actual domain
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + "/Success",
                CancelUrl = domain + "/Cancel"
            };

            foreach (var item in cart)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "inr",
                        UnitAmount = (long)(item.Price * 100), // Convert ₹ to paise
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Quantity
                });
            }

            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect to the Stripe checkout session URL
            return Redirect(session.Url);
        }


        public IActionResult Success()
        {
            HttpContext.Session.Remove("Cart");
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Shop(string searchTerm)
        {
            List<LocalProduct> products = new();
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();

                string query = string.IsNullOrEmpty(searchTerm)
                    ? "SELECT * FROM Product"
                    : "SELECT * FROM Product WHERE Name LIKE @searchTerm";

                SqlCommand cmd = new(query, conn);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new LocalProduct
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Photo = reader["Photo"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"])
                    });
                }
            }

            ViewBag.SearchTerm = searchTerm;
            return View(products);
        }


    }
}
