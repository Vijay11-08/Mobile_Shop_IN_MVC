using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MobileShopInMVC.Models
{
    public class Ratings
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        public string? Feedback { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
