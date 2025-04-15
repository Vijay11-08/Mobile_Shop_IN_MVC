using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileShopInMVC.Models
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
