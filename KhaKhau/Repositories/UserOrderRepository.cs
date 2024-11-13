using KhaKhau.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Repositories
{
    public class UserOrderRepository:IUserOrderRepository
    {
        private readonly KhaKhauContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserOrderRepository(KhaKhauContext context, IHttpContextAccessor httpContextAccessor
            ,UserManager<ApplicationUser> userManager)
        {
            _context = context;  
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _context.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetail)
                           .ThenInclude(x => x.Product)
                           .ThenInclude(x => x.Category).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _context.Orders.FindAsync(data.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{data.OrderId} does not found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _context.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await _context.SaveChangesAsync();
        }
    }
}
