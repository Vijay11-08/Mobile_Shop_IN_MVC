using Microsoft.Data.SqlClient;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }  // Foreign Key - Register table
        public int ProductId { get; set; } // Foreign Key - Product table
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductName { get; set; } // For display
        public string ProductImage { get; set; } // For display

        // Database Connection
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;");

        // Fetch Cart Items
        public List<Cart> GetCartItems(int userId)
        {
            List<Cart> cartItems = new List<Cart>();

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;"))
            {
                string query = "SELECT * FROM Cart WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cartItems.Add(new Cart
                    {
                        CartId = Convert.ToInt32(reader["CartId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TotalPrice = Convert.ToDecimal(reader["Price"])
                    });
                }
                reader.Close();
            }
            return cartItems;
        }

        // Add to Cart
        public bool AddToCart(int userId, int productId, int quantity, decimal price)
        {
            string query = "INSERT INTO Cart (UserId, ProductId, Quantity, TotalPrice) VALUES (@userId, @productId, @quantity, @totalPrice)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@totalPrice", quantity * price);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Update Cart Item Quantity
        public bool UpdateCart(int cartId, int quantity, decimal price)
        {
            string query = "UPDATE Cart SET Quantity = @quantity, TotalPrice = @totalPrice WHERE CartId = @cartId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cartId", cartId);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@totalPrice", quantity * price);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Remove from Cart
        public bool RemoveFromCart(int cartId)
        {
            string query = "DELETE FROM Cart WHERE CartId = @cartId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cartId", cartId);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Clear Cart (After Checkout)
        public bool ClearCart(int userId)
        {
            string query = "DELETE FROM Cart WHERE UserId = @userId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }
    }
}
