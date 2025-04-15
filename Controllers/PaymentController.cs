using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Collections.Generic;

namespace MobileShopInMVC.Controllers
{
    public class PaymentController : Controller
    {
        

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(string productName, decimal productPrice)
        {
            var domain = "https://localhost:44300"; // Replace with your deployed domain if needed

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = productPrice * 100, // amount in paise
                    Currency = "inr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = productName,
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = domain + "/Payment/Success",
                CancelUrl = domain + "/Payment/Cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

    }
}
