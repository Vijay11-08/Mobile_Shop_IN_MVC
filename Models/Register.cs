using Microsoft.Data.SqlClient;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  // Admin or User

        // Database Connection String
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


 

        public Register GetUser(string email, string password)
        {
            Register user = null;

            using (SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                conn.Open();
                string query = "SELECT * FROM Register WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new Register
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            return user;
        }

        // Fetch All or Specific Register Data
        public List<Register> getData(string id)
        {
            List<Register> lstreg = new List<Register>();
            string query = "SELECT * FROM Register";
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = "SELECT * FROM Register WHERE Id = " + id;
            }

            SqlDataAdapter apt = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstreg.Add(new Register
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Name = dr["Name"].ToString(),
                    Email = dr["Email"].ToString(),
                    Password = dr["Password"].ToString(),
                    Role = dr["Role"].ToString()
                });
            }
            return lstreg;
        }

        // Insert Data Into Register Table
        public bool insert(Register reg)
        {
            if (!string.IsNullOrEmpty(reg.Name) && !string.IsNullOrEmpty(reg.Email) && !string.IsNullOrEmpty(reg.Password) && !string.IsNullOrEmpty(reg.Role))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Register (Name, Email, Password, Role) VALUES (@name, @email, @password, @role)", con);
                cmd.Parameters.AddWithValue("@name", reg.Name);
                cmd.Parameters.AddWithValue("@email", reg.Email);
                cmd.Parameters.AddWithValue("@password", reg.Password);
                cmd.Parameters.AddWithValue("@role", reg.Role);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
            return false;
        }

        // Update Data In Register Table
        public bool update(Register reg)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Register SET Name=@name, Email=@email, Password=@password, Role=@role WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@name", reg.Name);
            cmd.Parameters.AddWithValue("@email", reg.Email);
            cmd.Parameters.AddWithValue("@password", reg.Password);
            cmd.Parameters.AddWithValue("@role", reg.Role);
            cmd.Parameters.AddWithValue("@Id", reg.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Delete Data From Register Table
        public bool delete(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Register WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // User Login Validation
        public Register login(string email, string password)
        {
            Register user = null;
            string query = "SELECT * FROM Register WHERE Email=@Email AND Password=@Password";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                user = new Register()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString(),
                    Role = reader["Role"].ToString()
                };
            }

            reader.Close();
            con.Close();

            return user;
        }
    }
}
