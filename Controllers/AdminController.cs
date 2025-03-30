using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MobileShopInMVC.Models;


namespace MobileShopInMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Category categoryModel = new Category();
        private readonly Product _productModel = new Product();
        private readonly Category _categoryModel;
       

        // List Categories
        public IActionResult Index()
        {
            var categories = categoryModel.GetCategories();
            return View(categories);
        }

        public AdminController()
        {

            _categoryModel = new Category();
        }

        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;";

        // GET: Manage Users
        public IActionResult ManageUsers()
        {
            List<Register> users = new List<Register>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Register";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new Register
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }
            return View(users);
        }

        // GET: Edit User
        public IActionResult EditUser(int id)
        {
            Register user = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Register WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new Register
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Edit User
        [HttpPost]
        public IActionResult EditUser(Register user)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "UPDATE Register SET Name=@Name, Email=@Email, Role=@Role WHERE Id=@Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@Id", user.Id);

                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("ManageUsers");
            }
            return View(user);
        }

        // GET: Delete User Confirmation
        public IActionResult DeleteUser(int id)
        {
            Register user = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Register WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new Register
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Confirm User Deletion
        [HttpPost, ActionName("DeleteUser")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "DELETE FROM Register WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("ManageUsers");
        }

        // Add Category (GET)
        public IActionResult AddCategory()
        {
            return View();
        }

        // Add Category (POST)
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                bool success = _categoryModel.Insert(category);
                if (success)
                    return RedirectToAction("Index");
            }
            return View(category);
        }

        // Edit Category (GET)
        public IActionResult EditCategory(int id)
        {
            var category = _categoryModel.GetData(id.ToString()).FirstOrDefault();
            if (category == null)
                return NotFound();

            return View(category);
        }

        // Edit Category (POST)
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                bool success = _categoryModel.Update(category);
                if (success)
                    return RedirectToAction("Index");
            }
            return View(category);
        }

        // Delete Category
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryModel.GetData(id.ToString()).FirstOrDefault();
            if (category == null)
                return NotFound();

            bool success = _categoryModel.Delete(id);
            if (success)
                return RedirectToAction("Index");

            return View("Index", _categoryModel.GetCategories());
        }


        

        // Delete Category (POST)
        [HttpPost]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            categoryModel.Delete(id);
            return RedirectToAction("Index");
        }
       
        public IActionResult Dashboard()
        {
            return View();
        }

        // Manage Categories
        public IActionResult ManageCategories()
        {
            Category category = new Category();
            List<Category> categories = category.GetCategories(); // Fetch categories from the model
            return View(categories); // Pass data to the view
        }


        // Manage Products
        public IActionResult ManageProducts()
        {
            Product product = new Product();
            List<Product> products = product.GetProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.Categories = new Category().GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _productModel.Insert(product);
                if (isInserted)
                    return RedirectToAction("ManageProducts");
            }
            ViewBag.Categories = new Category().GetCategories();
            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            Product product = _productModel.GetProducts().FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = new Category().GetCategories();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = _productModel.Update(product);
                if (isUpdated)
                    return RedirectToAction("ManageProducts");
            }
            ViewBag.Categories = new Category().GetCategories();
            return View(product);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _productModel.GetProducts().FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public IActionResult ConfirmDelete(int id)
        {
            bool isDeleted = _productModel.Delete(id);
            if (isDeleted)
                return RedirectToAction("ManageProducts");
            return View();
        }

        // Manage Orders
        public IActionResult ManageOrders()
        {
            Order order = new Order();
            List<Order> orders = order.getOrders();
            return View(orders);
        }

        // Manage Order Details
        public IActionResult ManageOrderDetails()
        {
            OrderItems orderDetails = new OrderItems();
            List<OrderItems> orderDetailsList = orderDetails.GetOrderItems(); // Fetch order details
            return View(orderDetailsList); // Pass data to the view
        }
        // Manage Payments
        public IActionResult ManagePayments()
        {
            Payment payment = new Payment();
            List<Payment> payments = payment.getPayments();
            return View(payments);
        }
        public IActionResult ViewCart()
        {
            int userId = GetLoggedInUserId(); // Fetch the logged-in user ID

            Cart cart = new Cart();
            List<Cart> cartItems = cart.GetCartItems(userId); // Pass userId

            return View(cartItems);
        }

        private int GetLoggedInUserId()
        {
            // Replace with actual user authentication logic
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return (int)HttpContext.Session.GetInt32("UserId");
            }
            return 0; // Return 0 if user is not logged in
        }
       


    }
}
