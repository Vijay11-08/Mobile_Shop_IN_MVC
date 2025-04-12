using Microsoft.EntityFrameworkCore;
using MobileShopInMVC.Models;  // Ensure correct namespace for Models

namespace MobileShopInMVC.Data  // Ensure this namespace matches the one in the controller
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Category> Category { get; set; }
 
        public DbSet<Ratings> Ratings { get; set; }


    }
}
