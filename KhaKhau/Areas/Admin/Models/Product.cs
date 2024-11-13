using KhaKhau.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhaKhau.Areas.Admin.Models
{
    public class Product
    {
        //ID int - auto icreasement and set primary key
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0.01, 1000000000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
		//update 
		public List<OrderDetail> OrderDetail { get; set; }
		public List<CartDetail> CartDetail { get; set; }
		[NotMapped]
		public string CategoryName { get; set; }
		[NotMapped]
		public int Quantity { get; set; }
        //update for stock 2/11
        public Stock Stock { get; set; }
	}
}
