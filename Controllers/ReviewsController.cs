using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Data;
using MobileShopInMVC.Models;
using System.IO;
using System;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace MobileShopInMVC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var reviews = _context.Review.OrderByDescending(r => r.CreatedAt).ToList();
            return View(reviews);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Review.Add(review);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        public IActionResult Details(int id)
        {
            var review = _context.Review.FirstOrDefault(r => r.Id == id);
            if (review == null) return NotFound();
            return View(review);
        }

        public IActionResult Delete(int id)
        {
            var review = _context.Review.Find(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _context.Review.Find(id);
            if (review != null)
            {
                _context.Review.Remove(review);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var review = _context.Review.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Review.Any(r => r.Id == review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

    }
}
