using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
namespace MobileShopInMVC.Models
{
    public class BuyerViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPhoto { get; set; }
        public decimal ProductPrice { get; set; }

        // Buyer info
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerAddress { get; set; }
    }
}
