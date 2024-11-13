namespace KhaKhau.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders(bool getAll = false);

        Task ChangeOrderStatus(UpdateOrderStatusModel data);
        Task<Order?> GetOrderById(int id);
        Task<IEnumerable<OrderStatus>> GetOrderStatuses();
        Task TogglePaymentStatus(int orderId);

    }
}