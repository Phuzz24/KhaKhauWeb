using KhaKhau.Repositories;
using Microsoft.AspNetCore.Mvc;
using KhaKhau.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
namespace KhaKhau.Controllers
{
    
    public class ProductController : Controller
    {
        //user
       

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(string sterm = "", int categoryId = 0)
        {
            // var products = await _productRepository.GetAllAsync();
            var products = await _productRepository.GetProduct(sterm, categoryId);
            //  var category = await _categoryRepository.GetAllAsync();
            var category = await _productRepository.Categories();
            ProDisplayModel proModel = new ProDisplayModel
            {
                Products = products,
                Categories = category,
                STerm = sterm,
                CategoryId = categoryId
            };

            return View(proModel);
        }

        //sign in to dat ban
        public ActionResult login()
        {
            return RedirectToPage("~/Identity/Account/Login");
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
    }
}
