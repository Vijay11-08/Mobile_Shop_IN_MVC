//using Intuit.Ipp.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } // Store image URL
        public int CategoryId { get; set; } // Foreign Key
        public string CategoryName { get; set; } // For display purpose

        public string PaymentId { get; set; }

        //public PaymentStatusEnum PaymentStatus { get; set; } // ✅ Use Enum
        // Database Connection
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;");

        // Fetch Products
        public List<Product> GetProducts(int? categoryId = null)
        {
            List<Product> productList = new List<Product>();
            string query = "SELECT p.*, c.Name AS CategoryName FROM Products p INNER JOIN Categories c ON p.CategoryId = c.CategoryId";

            if (categoryId.HasValue)
                query += " WHERE p.CategoryId = " + categoryId.Value;

            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                productList.Add(new Product
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = Convert.ToDecimal(dr["Price"]),
                    ImageUrl = dr["ImageUrl"].ToString(),
                    CategoryId = Convert.ToInt32(dr["CategoryId"]),
                    CategoryName = dr["CategoryName"].ToString()
                });
            }
            return productList;
        }

        // Insert Product
        public bool Insert(Product product)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId) VALUES (@name, @desc, @price, @image, @categoryId)", con);
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@desc", product.Description);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@image", product.ImageUrl);
            cmd.Parameters.AddWithValue("@categoryId", product.CategoryId);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Update Product
        public bool Update(Product product)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Products SET Name=@name, Description=@desc, Price=@price, ImageUrl=@image, CategoryId=@categoryId WHERE ProductId=@ProductId", con);
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@desc", product.Description);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@image", product.ImageUrl);
            cmd.Parameters.AddWithValue("@categoryId", product.CategoryId);
            cmd.Parameters.AddWithValue("@ProductId", product.ProductId); // Fixed Here

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Delete Product
        public bool Delete(int ProductId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductId=@ProductId", con);
            cmd.Parameters.AddWithValue("@ProductId", ProductId); // Fixed Here

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }
    }
}
