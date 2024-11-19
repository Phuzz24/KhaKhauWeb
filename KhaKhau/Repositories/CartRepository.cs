using KhaKhau.Areas.Identity.Data;
using KhaKhau.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly KhaKhauContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartRepository> _logger; // Thêm ILogger


        public CartRepository(KhaKhauContext context, IHttpContextAccessor httpContextAccessor,
       UserManager<ApplicationUser> userManager, ILogger<CartRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger; // Khởi tạo ILogger


        }
        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                // cart detail section
                var cartItem = _context.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _context.Products.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = product.Price  // it is a new line after update
                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }


        public async Task<int> RemoveItem(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                // cart detail section
                var cartItem = _context.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new InvalidOperationException("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _context.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove item from cart: {Message}", ex.Message);
                throw;
            }
            return await GetCartItemCount(userId);
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                // Trả về null hoặc một giỏ hàng trống nếu người dùng chưa đăng nhập
                _logger.LogWarning("User is not logged in, cannot retrieve shopping cart.");
                return null;
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Product)
                .ThenInclude(a => a.Stock)
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Product)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            return shoppingCart;
        }


        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Bắt đầu đặt hàng cho người dùng.");
                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogError("Người dùng chưa đăng nhập.");
                    throw new UnauthorizedAccessException("Người dùng chưa đăng nhập");
                }

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    _logger.LogError("Giỏ hàng không hợp lệ cho user ID: {UserId}", userId);
                    throw new InvalidOperationException("Giỏ hàng của bạn trống hoặc không hợp lệ.");
                }

                var cartDetails = _context.CartDetails.Where(cd => cd.ShoppingCartId == cart.Id).ToList();
                if (cartDetails.Count == 0)
                {
                    _logger.LogError("Giỏ hàng trống cho user ID: {UserId}", userId);
                    throw new InvalidOperationException("Giỏ hàng của bạn trống.");
                }

                var choXacNhanStatus = _context.OrderStatuses.FirstOrDefault(s => s.StatusName == "choxacnhan");
                if (choXacNhanStatus == null)
                {
                    _logger.LogError("Trạng thái đơn hàng 'choxacnhan' không được tìm thấy.");
                    throw new InvalidOperationException("Trạng thái đơn hàng 'choxacnhan' chưa được thiết lập trong hệ thống.");
                }

                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address, // Đã ghép nối FullAddress trong controller
                    IsPaid = false,
                    OrderStatusId = choXacNhanStatus.Id
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cartDetails)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        _logger.LogError("Sản phẩm với ID {ProductId} không tồn tại.", item.ProductId);
                        throw new InvalidOperationException($"Sản phẩm {item.ProductId} không tồn tại.");
                    }

                    var stock = await _context.Stocks.FirstOrDefaultAsync(a => a.Productid == item.ProductId);
                    if (stock == null || item.Quantity > stock.Quantity)
                    {
                        _logger.LogError("Không đủ hàng cho sản phẩm ID {ProductId}.", item.ProductId);
                        throw new InvalidOperationException($"Không đủ hàng cho {product.Name}");
                    }

                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                    stock.Quantity -= item.Quantity;
                }

                _context.CartDetails.RemoveRange(cartDetails);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Đặt hàng thành công cho Order ID: {OrderId}", order.Id);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError("Đặt hàng thất bại: {Message}", ex.Message);
                throw new InvalidOperationException("Đặt hàng thất bại: " + ex.Message);
            }
        }

        // CartRepository.cs
        public async Task<List<Product>> GetTopSellingProductsAsync(int minSales = 5)
        {
            var topSellingProducts = await _context.OrderDetails
                .Where(od => od.Order != null) // Đảm bảo chỉ lấy các sản phẩm có đơn hàng
                .GroupBy(od => od.ProductId)
                .Where(g => g.Sum(od => od.Quantity) >= minSales)  // Chỉ chọn các sản phẩm có tổng số lượng bán >= 5
                .Select(g => g.Key) // Lấy ProductId của các sản phẩm bán chạy
                .ToListAsync();

            // Lấy chi tiết các sản phẩm bán chạy và bao gồm thông tin liên quan
            var products = await _context.Products
                .Where(p => topSellingProducts.Contains(p.Id))
                .Include(p => p.Category) // Chỉ Include sau khi đã lọc đúng sản phẩm
                .ToListAsync();

            return products;
        }

        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public async Task<Order> GetLatestOrderForUser(string userId)
        {
            return await _context.Orders
                .Where(order => order.UserId == userId)
                .OrderByDescending(order => order.CreateDate)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Order>> GetOrderHistory(string userId, string orderCode = null, int? orderStatus = null)
        {
            var query = _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetail)
                .Where(o => o.UserId == userId);

            if (!string.IsNullOrEmpty(orderCode))
            {
                query = query.Where(o => o.Id.ToString().Contains(orderCode));
            }

            if (orderStatus.HasValue)
            {
                query = query.Where(o => o.OrderStatusId == orderStatus);
            }

            return await query.ToListAsync();
        }


        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetail)
                    .ThenInclude(d => d.Product) // Tải dữ liệu từ bảng Product
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }




        public async Task<bool> CancelOrder(int orderId, string userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null || order.OrderStatusId != 1) // Chỉ cho phép hủy khi trạng thái là "Chờ xác nhận"
            {
                return false;
            }

            order.OrderStatusId = 1002; // Mặc định "đã hủy" là 1002
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }






    }
}

