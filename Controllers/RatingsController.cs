using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Data;
using MobileShopInMVC.Models;
using System.IO;
using System;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace MobileShopInMVC.Controllers
{
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ratings = _context.Ratings.OrderByDescending(r => r.CreatedAt).ToList();
            return View(ratings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ratings rating)
        {
            if (ModelState.IsValid)
            {
                _context.Ratings.Add(rating);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }

        public IActionResult Details(int id)
        {
            var rating = _context.Ratings.FirstOrDefault(r => r.Id == id);
            if (rating == null) return NotFound();
            return View(rating);
        }

        public IActionResult Delete(int id)
        {
            var rating = _context.Ratings.Find(id);
            if (rating == null) return NotFound();
            return View(rating);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var rating = _context.Ratings.Find(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Ratings/Edit/5
        public IActionResult Edit(int id)
        {
            var rating = _context.Ratings.Find(id);
            if (rating == null)
            {
                return NotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Ratings rating)
        {
            if (id != rating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(rating);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(rating);
        }

    }
}
