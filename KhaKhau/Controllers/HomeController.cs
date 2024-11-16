using KhaKhau.Areas.Identity.Data;
using KhaKhau.Models;
using KhaKhau.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KhaKhau.Controllers
{


    
    public class HomeController : Controller
    {
        
       
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            _logger = logger;
            this._userManager = userManager;
            _cartRepository = cartRepository;

        }
        public async Task<IActionResult> Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            var topSellingProducts = await _cartRepository.GetTopSellingProductsAsync();

            // Lấy dữ liệu từ ShoppingCart nếu cần
            var shoppingCart = await _cartRepository.GetUserCart();
            ViewData["CartItemCount"] = shoppingCart?.CartDetails.Count ?? 0; // Ví dụ: số lượng sản phẩm trong giỏ hàng

            var viewModel = new HomeViewModel
            {
                TopSellingProducts = topSellingProducts
            };

            return View(viewModel);
        }



        [Authorize(Roles ="user")]
        
        public IActionResult Privacy()
        {
            return View();
        } 
        public IActionResult temptList()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult GioiThieu()
        {
            return View();
               
        }
        public IActionResult DatBan()
        {
            return View();
        }
    }
}