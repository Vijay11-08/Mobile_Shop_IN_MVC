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

            public string CreateOrder(decimal amount)
            {
                // Logic to create Razorpay order
                return "razorpay_order_id";  // Dummy return for now
            }

            public string GetKeyId()  // ✅ Ensure this method exists
            {
                return _keyId;
            }
        }
    }

