using System.ComponentModel.DataAnnotations;

namespace KhaKhau.Models
{
    public class StockDTO
    {
        public int ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "So luong san pham khong am")]
        public int Quantity { get; set; }
    }
}
