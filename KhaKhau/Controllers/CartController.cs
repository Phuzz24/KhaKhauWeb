using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KhaKhau.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(productId, qty);
            if (redirect == 0)
            {
                return Ok(cartCount);
            }
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepository.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepository.GetCartItemCount();
            return Ok(cartItem);
        }

        [HttpGet] // Thêm HttpGet để rõ ràng rằng đây là phương thức GET
        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartRepository.GetUserCart();
            if (cart == null || cart.CartDetails == null || !cart.CartDetails.Any())
            {
                ModelState.AddModelError(string.Empty, "Giỏ hàng của bạn đang trống.");
                return RedirectToAction("GetUserCart");
            }

            var model = new CheckoutModel
            {
                CartItems = cart.CartDetails.Select(item => new CartItem
                {
                    Name = item.Product.Name,
                    ImageUrl = item.Product.ImageUrl,
                    Quantity = item.Quantity,
                    UnitPrice = (double)item.UnitPrice
                }).ToList(),
                TotalAmount = cart.CartDetails.Sum(item => item.Quantity * (double)item.UnitPrice)
            };

            return View(model);
        }

        [HttpPost] // Thêm HttpPost để rõ ràng rằng đây là phương thức POST
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.Address = $"{model.Address}, {model.Ward}, {model.District}, {model.Province}";

                bool isCheckedOut = await _cartRepository.DoCheckout(model);
                if (!isCheckedOut)
                {
                    ModelState.AddModelError(string.Empty, "Đặt hàng thất bại.");
                    return View(model);
                }

                // Get the latest order for the user
                var userId = _cartRepository.GetUserId(); // Or fetch from context if GetUserId is private
                var order = await _cartRepository.GetLatestOrderForUser(userId);

                // Kiểm tra xem `order` có giá trị hay không trước khi truy cập thuộc tính của nó
                if (order != null)
                {
                    TempData["OrderId"] = order.Id;
                    TempData["OrderUserName"] = model.Name; // Sử dụng tên người nhận từ `CheckoutModel`
                    TempData["OrderAddress"] = model.Address;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể tìm thấy đơn hàng.");
                    return RedirectToAction("OrderFailure");
                }

                return RedirectToAction(nameof(OrderSuccess));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}
