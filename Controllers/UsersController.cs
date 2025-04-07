using Microsoft.AspNetCore.Mvc;
using MobileShopInMVC.Models;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MobileShopInMVC.Controllers
{
    public class UsersController : Controller
    {
        private string connectionString = "Server=(localdb)\\MSSQLLocalDB; Database=MOBILESHOP; Trusted_Connection=True;";
        private EmailService emailService = new EmailService();

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users user)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string token = Guid.NewGuid().ToString();
                string query = "INSERT INTO `Users` (FullName, Email, PasswordHash, Gender, Mobile, ProfilePic, Token, IsVerified) VALUES (@FullName, @Email, @PasswordHash, @Gender, @Mobile, @ProfilePic, @Token, @IsVerified)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@Mobile", user.Mobile);
                cmd.Parameters.AddWithValue("@ProfilePic", user.ProfilePic);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@IsVerified", 'N');

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                string verificationLink = Url.Action("VerifyEmail", "Users", new { email = user.Email, token = token }, Request.Scheme);
                await emailService.SendEmailAsync(user.Email, "Verify Your Email", $"Click <a href='{verificationLink}'>here</a> to verify your email.");
            }
            return RedirectToAction("Login");
        }

        public IActionResult VerifyEmail(string email, string token)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "UPDATE `Users` SET IsVerified = 'Y' WHERE Email = @Email AND Token = @Token";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Token", token);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                    return RedirectToAction("Login");
                else
                    return Content("Invalid verification link.");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM `Users` WHERE Email = @Email AND PasswordHash = @PasswordHash AND IsVerified = 'Y'";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", password);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                bool isAuthenticated = reader.Read();
                con.Close();

                if (isAuthenticated)
                    return RedirectToAction("Dashboard");
                else
                    ViewBag.Message = "Invalid Credentials or Account not verified.";
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            string token = Guid.NewGuid().ToString();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "UPDATE `Users` SET Token = @Token WHERE Email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Token", token);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            string resetLink = Url.Action("ResetPassword", "Users", new { email = email, token = token }, Request.Scheme);
            await emailService.SendEmailAsync(email, "Reset Your Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");
            return View("CheckYourEmail");
        }

        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string email, string token, string newPassword)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "UPDATE `Users` SET PasswordHash = @PasswordHash WHERE Email = @Email AND Token = @Token";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@PasswordHash", newPassword);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Login");
        }
    }
}
