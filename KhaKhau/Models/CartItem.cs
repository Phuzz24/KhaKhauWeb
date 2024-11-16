
namespace KhaKhau.Models
{
    public class CartItem
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; } // Đổi từ decimal sang double
        public double TotalPrice => Quantity * UnitPrice; // Tính tự động từ Quantity và UnitPrice
    }

}
