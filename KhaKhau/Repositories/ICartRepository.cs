namespace KhaKhau.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int productId, int qty);
        Task<int> RemoveItem(int productId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<bool> DoCheckout(CheckoutModel model);
        Task<List<Product>> GetTopSellingProductsAsync(int minSales = 5);

        Task<Order> GetLatestOrderForUser(string userId);
        string GetUserId();
        Task<List<Order>> GetOrderHistory(string userId, string orderCode = null, int? orderStatus = null);
        Task<Order> GetOrderById(int orderId);
        Task<bool> CancelOrder(int orderId, string userId);
    }
}
