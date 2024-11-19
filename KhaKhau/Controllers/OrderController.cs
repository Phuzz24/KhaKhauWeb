using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using KhaKhau.Models;
using System.Security.Claims;
namespace KhaKhau.Controllers
{

    [Authorize]

    public class OrderController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserService _userService;

        public OrderController(ICartRepository cartRepository, IUserService userService)
        {
            _cartRepository = cartRepository;
            _userService = userService;
        }
        public async Task<IActionResult> OrderHistory(string searchOrderCode, int? statusId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _cartRepository.GetOrderHistory(userId, searchOrderCode, statusId);
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var order = await _cartRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _cartRepository.CancelOrder(orderId, userId);

            if (success)
            {
                TempData["Message"] = "Đơn hàng đã được hủy thành công.";
                return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng. Vui lòng thử lại.";
                return Json(new { success = false, message = "Không thể hủy đơn hàng. Vui lòng thử lại." });
            }
        }


    }
}
