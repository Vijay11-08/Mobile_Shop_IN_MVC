using Razorpay.Api;

namespace MobileShopInMVC.Services
{
    public class RazorpayService
    {
        private readonly string _keyId;
        private readonly string _keySecret;

        public RazorpayService(IConfiguration configuration)
        {
            _keyId = configuration["Razorpay:KeyId"];
            _keySecret = configuration["Razorpay:KeySecret"];
        }

        public string CreateOrder(decimal amount, string currency = "INR")
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_keyId, _keySecret);
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", amount * 100 }, // Razorpay works with paise
                    { "currency", currency },
                    { "payment_capture", 1 }
                };

                Order order = client.Order.Create(options);
                return order["id"].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Razorpay order: " + ex.Message);
            }
        }
    }
}
