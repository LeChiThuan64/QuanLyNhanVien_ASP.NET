namespace YourProjectNamespace.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? PhongId { get; set; }
        public int Role { get; set; }
        public string HoVaTen { get; set; } = "Chưa có tên"; // Giá trị mặc định
        public string Email { get; set; } = "Không có email"; // Giá trị mặc định
    }
}
