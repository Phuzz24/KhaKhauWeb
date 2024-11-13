using KhaKhau.Repositories;
using Microsoft.AspNetCore.Mvc;
using KhaKhau.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
namespace KhaKhau.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class UserManagerController : Controller
    {
        private readonly IUserResponsitory _userReponsitory;
        public UserManagerController(IUserResponsitory userReponsitory)
        {
            _userReponsitory = userReponsitory;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userReponsitory.GetAllAsync();
            return View(user);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userReponsitory.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userReponsitory.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
