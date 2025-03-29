using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MobileShopInMVC.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; } // Primary Key

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; } // Foreign Key (Links to Order table)

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // Payment Method (Credit Card, UPI, etc.)

        [StringLength(255)]
        public string TransactionId { get; set; } // Unique transaction ID for online payments

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Payment status (Pending, Success, Failed)

        [Required]
        public decimal PaidAmount { get; set; } // Total paid amount

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default timestamp

        // Navigation Property
        public virtual Order Order { get; set; }

        // Database Connection
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MOBILESHOP;Integrated Security=True;");

        // Fetch Payment Details (All or Specific Order)
        public List<Payment> getPayments(string id = null)
        {
            List<Payment> payments = new List<Payment>();
            string query = "SELECT * FROM Payments";
            if (!string.IsNullOrWhiteSpace(id))
            {
                query += " WHERE OrderId = " + id;
            }

            SqlDataAdapter apt = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                payments.Add(new Payment
                {
                    PaymentId = Convert.ToInt32(dr["PaymentId"]),
                    OrderId = Convert.ToInt32(dr["OrderId"]),
                    PaymentMethod = dr["PaymentMethod"].ToString(),
                    TransactionId = dr["TransactionId"].ToString(),
                    Status = dr["Status"].ToString(),
                    PaidAmount = Convert.ToDecimal(dr["PaidAmount"]),
                    CreatedAt = Convert.ToDateTime(dr["CreatedAt"])
                });
            }
            return payments;
        }

        // Insert Payment
        public bool insert(Payment payment)
        {
            if (payment.OrderId > 0 && payment.PaidAmount > 0)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Payments (OrderId, PaymentMethod, TransactionId, Status, PaidAmount, CreatedAt) VALUES (@OrderId, @PaymentMethod, @TransactionId, @Status, @PaidAmount, @CreatedAt)", con);
                cmd.Parameters.AddWithValue("@OrderId", payment.OrderId);
                cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                cmd.Parameters.AddWithValue("@TransactionId", string.IsNullOrWhiteSpace(payment.TransactionId) ? DBNull.Value : (object)payment.TransactionId);
                cmd.Parameters.AddWithValue("@Status", payment.Status);
                cmd.Parameters.AddWithValue("@PaidAmount", payment.PaidAmount);
                cmd.Parameters.AddWithValue("@CreatedAt", payment.CreatedAt);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
            return false;
        }

        // Update Payment
        public bool update(Payment payment)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Payments SET OrderId=@OrderId, PaymentMethod=@PaymentMethod, TransactionId=@TransactionId, Status=@Status, PaidAmount=@PaidAmount WHERE PaymentId=@PaymentId", con);
            cmd.Parameters.AddWithValue("@OrderId", payment.OrderId);
            cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
            cmd.Parameters.AddWithValue("@TransactionId", string.IsNullOrWhiteSpace(payment.TransactionId) ? DBNull.Value : (object)payment.TransactionId);
            cmd.Parameters.AddWithValue("@Status", payment.Status);
            cmd.Parameters.AddWithValue("@PaidAmount", payment.PaidAmount);
            cmd.Parameters.AddWithValue("@PaymentId", payment.PaymentId);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }

        // Delete Payment
        public bool delete(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Payments WHERE PaymentId=@PaymentId", con);
            cmd.Parameters.AddWithValue("@PaymentId", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
        }
    }
}
