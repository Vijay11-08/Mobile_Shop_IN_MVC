using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Models;

namespace MobileShopInMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Category _categoryModel;

        public CategoryController()
        {
            _categoryModel = new Category();
        }

        // Display list of categories
        public IActionResult Index()
        {
            var categories = _categoryModel.GetCategories();
            return View(categories);
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
    }
    }
