using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Models;
using System.Security.Claims;

namespace MobileShopInMVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly Register _register;

        public AccountController()
        {
            _register = new Register();
        }

        public IActionResult Login()
        {
            return View();
        }
        private readonly Register _registerModel = new Register();

        // Register Action
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register reg)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _registerModel.insert(reg);
                if (isInserted)
                {
                    TempData["SuccessMessage"] = "Registration successful. You can now log in.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Registration failed. Please try again.";
                }
            }
            return View(reg);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loggedUser = _register.GetUser(email, password);

            if (loggedUser != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loggedUser.Name),
            new Claim(ClaimTypes.Email, loggedUser.Email),
            new Claim(ClaimTypes.Role, loggedUser.Role) // Store role in claims
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (loggedUser.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Invalid Email or Password!";
                return View();
            }
        }
        // GET: Forgot Password Page
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Forgot Password Action
        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            var user = _registerModel.getData("").FirstOrDefault(u => u.Email == Email);
            if (user != null)
            {
                // Here you can implement email sending logic for password reset
                ViewBag.Message = "Password reset instructions have been sent to your email.";
            }
            else
            {
                ViewBag.ErrorMessage = "Email not found!";
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        
    }
}
