using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly KhaKhauContext _context;

        public StatisticsController(KhaKhauContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Thống kê đơn hàng
            var today = DateTime.Today;
            var ordersToday = await _context.Orders.Where(o => o.CreateDate.Date == today).CountAsync();
            var ordersLast7Days = await _context.Orders.Where(o => o.CreateDate >= today.AddDays(-7) && o.CreateDate < today).CountAsync();
            var ordersLast10Days = await _context.Orders.Where(o => o.CreateDate >= today.AddDays(-10) && o.CreateDate < today.AddDays(-7)).CountAsync();

            // Thống kê sản phẩm bán chạy nhất
            var topSellingProducts = await _context.OrderDetails
                .Include(od => od.Product)
                .GroupBy(od => new { od.ProductId, od.Product.Name })
                .Select(g => new TopSellingProductModel
                {
                    ProductName = g.Key.Name,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(5)
                .ToListAsync();

            // Thống kê tồn kho
            var stockSummary = await _context.Stocks
                .Include(s => s.Product)
                .Select(s => new StockSummaryModel
                {
                    ProductName = s.Product.Name,
                    Quantity = s.Quantity
                })
                .ToListAsync();

            // Đưa dữ liệu vào ViewModel
            var model = new StatisticsViewModel
            {
                OrdersToday = ordersToday,
                OrdersLast7Days = ordersLast7Days,
                OrdersLast10Days = ordersLast10Days,
                TopSellingProducts = topSellingProducts,
                StockSummary = stockSummary
            };

            return View(model);
        }


    }
}
