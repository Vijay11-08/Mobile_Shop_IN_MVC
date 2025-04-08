namespace MobileShopInMVC.Models
{
   public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Product != null ? Product.Price * Quantity : 0;
    }
}
