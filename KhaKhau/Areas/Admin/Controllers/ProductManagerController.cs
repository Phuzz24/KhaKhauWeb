using Microsoft.AspNetCore.Mvc;
using KhaKhau.Repositories;
using KhaKhau.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography.Xml;
using System.Runtime.CompilerServices;

namespace KhaKhau.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ProductManagerController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserOrderRepository _userOrderRepository;
        
        public ProductManagerController(IProductRepository productRepository,
        ICategoryRepository categoryRepository, IUserOrderRepository userOrderRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userOrderRepository = userOrderRepository;
        }
        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        //xu ly them san pham
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("CategoryName"); // Loại bỏ xác thực ModelState cho   ImageUrl
            ModelState.Remove("CartDetail");
            ModelState.Remove("OrderDetail");
			ModelState.Remove("Stock"); // Loại bỏ xác thực ModelState cho   ...

			if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Catagories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        //ham SaveImage
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }
        //Cap nhat
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        //Xu ly cap nhat 

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)

        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("CategoryName"); // Loại bỏ xác thực ModelState cho   ImageUrl
            ModelState.Remove("CartDetail");
            ModelState.Remove("OrderDetail"); // Loại bỏ xác thực ModelState cho   ...
            ModelState.Remove("Stock"); // Loại bỏ xác thực ModelState cho   ...


            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //else
                //    {
                var existingProduct = await
            _productRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
                                                 // Giữ nguyên thông tin hình ảnh nếu không có hình mới được  tải lên

                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                    // Cập nhật các thông tin khác của sản phẩm
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.Description = product.Description;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.ImageUrl = product.ImageUrl;
                    await _productRepository.UpdateAsync(existingProduct);
                    return RedirectToAction(nameof(Index));
                }  

                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
          
                return View(product);


            
        }
		//chi tiet san pham
		public async Task<IActionResult> Display(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        //xu lu orrder 
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepository.UserOrders(true);
            return View(orders);
        }
    }
}
