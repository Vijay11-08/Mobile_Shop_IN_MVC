using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [ForeignKey("Register")]
        public int UserId { get; set; }  // Foreign Key linking to Register table (User who placed the order)

        [Required]
        public DateTime OrderDate { get; set; }  // Order Date

        [Required]
        public decimal TotalAmount { get; set; }  // Total Price of Order

        [Required]
        public string Status { get; set; } // Pending, Completed, Cancelled

        // Navigation Property
        public virtual Register User { get; set; }
        public virtual List<Order> OrderItems { get; set; }

        // Database Connection String
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;");

        // Fetch Orders (All or Specific)
        public List<Order> getOrders(string id = null)
        {
            List<Order> orders = new List<Order>();
            string query = "SELECT * FROM Orders";
            if (!string.IsNullOrWhiteSpace(id))
            {
                query += " WHERE Id = " + id;
            }

            SqlDataAdapter apt = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                orders.Add(new Order
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    UserId = Convert.ToInt32(dr["UserId"]),
                    OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                    Status = dr["Status"].ToString()
                });
            }
            return orders;
        }

        // Insert Order
        public bool insert(Order order)
        {
            if (order.UserId > 0 && order.TotalAmount > 0 && !string.IsNullOrEmpty(order.Status))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Orders (UserId, OrderDate, TotalAmount, Status) VALUES (@UserId, @OrderDate, @TotalAmount, @Status)", con);
                cmd.Parameters.AddWithValue("@UserId", order.UserId);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@Status", order.Status);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
            return false;
        }

        // Update Order
        public bool update(Order order)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Orders SET UserId=@UserId, OrderDate=@OrderDate, TotalAmount=@TotalAmount, Status=@Status WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@UserId", order.UserId);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            cmd.Parameters.AddWithValue("@Status", order.Status);
            cmd.Parameters.AddWithValue("@Id", order.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Delete Order
        public bool delete(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Orders WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }
    }
}
