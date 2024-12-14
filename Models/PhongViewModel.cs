namespace YourProjectNamespace.Models
{
    public class PhongViewModel
    {
        public int PhongId { get; set; }
        public string TenPhong { get; set; } = "Tên phòng chưa được đặt"; // Giá trị mặc định
        public string MoTa { get; set; } = "Không có mô tả"; // Giá trị mặc định
        public int SoLuongNguoi { get; set; }
        public List<UserViewModel> DanhSachNhanVien { get; set; } = new List<UserViewModel>();
    }
}
