using System.ComponentModel.DataAnnotations;

namespace MobileShopInMVC.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string ProfilePic { get; set; }
        public string Token { get; set; }
        public char IsVerified { get; set; } = 'N';
        public DateTime CreatedAt { get; set; }
    }
}
