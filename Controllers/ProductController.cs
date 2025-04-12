using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using MobileShopInMVC.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MobileShopInMVC.Data;
using Microsoft.Data.SqlClient;

namespace MobileShopInMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public ProductController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        // GET: Product
        public IActionResult Index()
        {
            List<Product> products = new();
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM Product", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
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
        public IActionResult Create(Product product)
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
            Product product = new();
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
        public IActionResult Edit(Product product)
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
    }
}
