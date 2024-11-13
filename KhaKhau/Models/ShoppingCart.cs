using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhaKhau.Models
{
	[Table("ShoppingCart")]


	//public List<CartItem> Items { get; set; } = new List<CartItem>();
	//public void AddItem(CartItem item)
	//{
	//    var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
	//    if (existingItem != null)
	//    {
	//        existingItem.Quantity += item.Quantity;

	//    }
	//    else
	//    {
	//        Items.Add(item);
	//    }
	//}
	//public void RemoveItem(int productId)
	//{
	//    Items.RemoveAll(i => i.ProductId == productId);
	//}

	public class ShoppingCart
	{
		public int Id { get; set; }
		[Required]
		public string UserId { get; set; }
		public bool IsDeleted { get; set; } = false;

		public ICollection<CartDetail> CartDetails { get; set; }

	}
}


