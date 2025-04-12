using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;


namespace MobileShopInMVC.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Photo { get; set; } // Store filename in the database

        [NotMapped]
        public IFormFile PhotoFile { get; set; } // Handle file upload

        public string Description { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal Price { get; set; }
  

    }
}
