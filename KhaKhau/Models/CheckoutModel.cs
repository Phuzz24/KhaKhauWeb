using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KhaKhau.Models
{
    public class CheckoutModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận.")]
        [MaxLength(30, ErrorMessage = "Tên không được quá 30 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [MaxLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; }

        // Các thuộc tính mới
        [Required(ErrorMessage = "Vui lòng chọn tỉnh/thành phố.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quận/huyện.")]
        public string District { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phường/xã.")]
        public string Ward { get; set; }

        // Giỏ hàng
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Khởi tạo mặc định
        public double TotalAmount { get; set; }
    }


}

