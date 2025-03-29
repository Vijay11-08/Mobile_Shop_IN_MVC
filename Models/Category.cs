using Microsoft.Data.SqlClient;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } // Store image path

        // Database Connection String
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;");

        // Method to fetch all categories
        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;"))
            {
                string query = "SELECT * FROM Categories";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        ImageUrl = reader["ImageUrl"].ToString()
                    });
                }
                reader.Close();
            }
            return categories;
        }

        // Fetch All or Specific Category Data
        public List<Category> GetData(string id = "")
        {
            List<Category> categories = new List<Category>();
            string query = string.IsNullOrWhiteSpace(id) ? "SELECT * FROM Categories" : "SELECT * FROM Categories WHERE Id = " + id;

            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                categories.Add(new Category
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    ImageUrl = dr["ImageUrl"].ToString()
                });
            }
            return categories;
        }

        // Insert Data Into Category Table
        public bool Insert(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) && !string.IsNullOrEmpty(category.Description) && !string.IsNullOrEmpty(category.ImageUrl))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Categories (Name, Description, ImageUrl) VALUES (@name, @desc, @image)", con);
                cmd.Parameters.AddWithValue("@name", category.Name);
                cmd.Parameters.AddWithValue("@desc", category.Description);
                cmd.Parameters.AddWithValue("@image", category.ImageUrl);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
            return false;
        }

        // Update Data In Category Table
        public bool Update(Category category)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Categories SET Name=@name, Description=@desc, ImageUrl=@image WHERE Id=@id", con);
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.Parameters.AddWithValue("@desc", category.Description);
            cmd.Parameters.AddWithValue("@image", category.ImageUrl);
            cmd.Parameters.AddWithValue("@id", category.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Delete Data From Category Table
        public bool Delete(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE Id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }
    }
}
