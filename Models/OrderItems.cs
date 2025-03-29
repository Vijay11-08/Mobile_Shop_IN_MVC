using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class OrderItems
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }  // Foreign Key (Links to Order table)

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }  // Foreign Key (Links to Product table)

        [Required]
        public int Quantity { get; set; }  // Quantity of the product

        [Required]
        public decimal Price { get; set; }  // Price per item

        [Required]
        public decimal TotalPrice { get; set; }  // Total Price (Quantity * Price)

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        // Database Connection String
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;");

        // Fetch Order Items (All or Specific Order)
        public List<OrderItems> GetOrderItems(string id = null)
        {
            List<OrderItems> orderItems = new List<OrderItems>();
            string query = "SELECT * FROM OrderItems";
            if (!string.IsNullOrWhiteSpace(id))
            {
                query += " WHERE OrderId = " + id;
            }

            SqlDataAdapter apt = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                orderItems.Add(new OrderItems
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    OrderId = Convert.ToInt32(dr["OrderId"]),
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    Quantity = Convert.ToInt32(dr["Quantity"]),
                    Price = Convert.ToDecimal(dr["Price"]),
                    TotalPrice = Convert.ToDecimal(dr["TotalPrice"])
                });
            }
            return orderItems;
        }

        // Insert Order Items
        public bool insert(OrderItems orderItems)
        {
            if (orderItems.OrderId > 0 && orderItems.ProductId > 0 && orderItems.Quantity > 0 && orderItems.Price > 0)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price, TotalPrice) VALUES (@OrderId, @ProductId, @Quantity, @Price, @TotalPrice)", con);
                cmd.Parameters.AddWithValue("@OrderId", orderItems.OrderId);
                cmd.Parameters.AddWithValue("@ProductId", orderItems.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", orderItems.Quantity);
                cmd.Parameters.AddWithValue("@Price", orderItems.Price);
                cmd.Parameters.AddWithValue("@TotalPrice", orderItems.Quantity * orderItems.Price);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
            return false;
        }

        // Update Order Items
        public bool update(OrderItems orderItems)
        {
            SqlCommand cmd = new SqlCommand("UPDATE OrderItems SET OrderId=@OrderId, ProductId=@ProductId, Quantity=@Quantity, Price=@Price, TotalPrice=@TotalPrice WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@OrderId", orderItems.OrderId);
            cmd.Parameters.AddWithValue("@ProductId", orderItems.ProductId);
            cmd.Parameters.AddWithValue("@Quantity", orderItems.Quantity);
            cmd.Parameters.AddWithValue("@Price", orderItems.Price);
            cmd.Parameters.AddWithValue("@TotalPrice", orderItems.Quantity * orderItems.Price);
            cmd.Parameters.AddWithValue("@Id", orderItems.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Delete Order Items
        public bool delete(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM OrderItems WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }
    }
}
