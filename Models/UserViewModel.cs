using System;
using System.ComponentModel.DataAnnotations;

namespace YourProjectNamespace.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }

        // Mật khẩu không bắt buộc khi cập nhật
        public string Password { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = "Không có email"; // Giá trị mặc định

        public DateTime? DateOfBirth { get; set; }

        public int? PhongId { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống.")]
        public int Role { get; set; }

        // Thuộc tính bổ sung cho hiển thị
        public string HoVaTen { get; set; } = "Chưa có tên"; // Giá trị mặc định
    }
}
