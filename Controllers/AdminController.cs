using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // GET: Edit User
        public IActionResult EditUser(int id)
        {
            Register user = new Register().getData(id.ToString()).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.RoleList = new List<SelectListItem>
    {
        new SelectListItem { Value = "Admin", Text = "Admin" },
        new SelectListItem { Value = "User", Text = "User" }
    };

            return View(user);
        }
        // POST: Edit User
        [HttpPost]
        public IActionResult EditUser(Register user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Registers.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;
                    _context.SaveChanges();
                    return RedirectToAction("ManageUsers");
                }
            }
            return View(user);
        }

        // GET: Delete User Confirmation
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Registers.FirstOrDefault(u => u.Id == id);
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
            var user = _context.Registers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Registers.Remove(user);
                _context.SaveChanges();
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

        // Manage Users
        public IActionResult ManageUsers()
        {
            Register register = new Register();
            List<Register> users = register.getData(""); // Fetch all users
            return View(users);
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
