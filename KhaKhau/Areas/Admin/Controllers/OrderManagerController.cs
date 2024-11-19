using KhaKhau.Repositories;
using Microsoft.AspNetCore.Mvc;
using KhaKhau.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class OrderManagerController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;
        public OrderManagerController(IUserOrderRepository userOrderRepository)
        {
            _userOrderRepository = userOrderRepository;
        }
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepository.UserOrders(true);
            return View(orders);
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
                // log exception here
            }
            return RedirectToAction(nameof(AllOrders));
        }

        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _userOrderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not found.");
            }
            var orderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus =>
            {
                return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = order.OrderStatusId == orderStatus.Id };
            });
            var data = new UpdateOrderStatusModel
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus =>
                    {
                        return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = orderStatus.Id == data.OrderStatusId };
                    });

                    return View(data);
                }
                await _userOrderRepository.ChangeOrderStatus(data);
                TempData["msg"] = "Updated successfully";
            }
            catch (Exception ex)
            {
                // catch exception here
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOrders([FromBody] List<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
            {
                return Json(new { success = false, message = "Không có đơn hàng nào được chọn." });
            }

            try
            {
                await _userOrderRepository.DeleteOrders(orderIds); // Thực hiện xóa các đơn hàng
                return Json(new { success = true, message = "Xóa thành công các đơn hàng đã chọn." });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa các đơn hàng." });
            }
        }




        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
