namespace KhaKhau.Models
{
    public class OrderDetailModal
    {
        public string DivId { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
   }
}
