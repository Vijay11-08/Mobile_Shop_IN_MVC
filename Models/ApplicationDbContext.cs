using Microsoft.EntityFrameworkCore;

namespace MobileShopInMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; } // ✅ Ensure this exist
        public DbSet<Payment> Payments { get; set; }

    }
}
