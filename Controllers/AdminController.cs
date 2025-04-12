using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MobileShopInMVC.Models;


namespace MobileShopInMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
     

        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;";


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }


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
    }
}
