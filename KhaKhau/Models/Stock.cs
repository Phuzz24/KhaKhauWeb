using System.ComponentModel.DataAnnotations.Schema;

namespace KhaKhau.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }
        public int Productid { get; set; }
        public int Quantity { get; set; }

        public Product? Product { get; set; }
    }
}