﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhongUserManagement.Models;
using System.Diagnostics;
using YourProjectNamespace.Models;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;


namespace PhongUserManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PhongUserDbContext _context;

        public HomeController(ILogger<HomeController> logger, PhongUserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult ThemUser()
        {
            // Truyền danh sách phòng vào ViewBag để hiển thị trong dropdown
            ViewBag.Phongs = _context.Phongs
                .Select(p => new { PhongId = p.PhongId, TenPhong = p.TenPhong })
                .ToList();

            return View();
        }


        [HttpGet]
		public IActionResult XoaPhong(int id)
		{
			// Tìm phòng cần xóa trong database
			var phong = _context.Phongs.FirstOrDefault(p => p.PhongId == id);

			if (phong != null)
			{
				// Xóa phòng
				_context.Phongs.Remove(phong);
				_context.SaveChanges();

				// Chuyển hướng lại trang danh sách phòng
				return RedirectToAction("Phong");
			}

			// Nếu phòng không tồn tại, chuyển hướng lại danh sách
			return RedirectToAction("Phong");
		}

		[HttpPost]
        [HttpPost]
        public IActionResult ThemPhong(string TenPhong, string MoTa, int? SoLuongNguoi)
        {
            if (string.IsNullOrEmpty(TenPhong))
            {
                ModelState.AddModelError("TenPhong", "Tên phòng không được để trống.");
                return View();
            }

            if (string.IsNullOrEmpty(MoTa))
            {
                ModelState.AddModelError("MoTa", "Mô tả không được để trống.");
                return View();
            }

            // Tạo đối tượng Phong mới
            var phongMoi = new Phong
            {
                TenPhong = TenPhong,
                MoTa = MoTa,
                SoLuongNguoi = SoLuongNguoi
            };

            // Lưu vào database
            _context.Phongs.Add(phongMoi);
            _context.SaveChanges();

            return RedirectToAction("Phong");
        }



        public IActionResult ThemPhong()
        {
            return View();
        }

        public IActionResult ExportToPdf()
        {
            // Lấy dữ liệu từ database
            var danhSachNhanVien = _context.Users
                .AsEnumerable() // Chuyển sang xử lý trên client
                .Select((u, index) => new
                {
                    STT = index + 1,
                    HoVaTen = u.FullName,
                    Email = u.Email,
                    TinhTrang = u.Role == 0 ? "Admin" : "User"
                })
                .ToList();

            // Tạo file PDF trong bộ nhớ
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Thêm tiêu đề
            document.Add(new Paragraph("Danh Sách Nhân Viên")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(18));

            // Tạo bảng PDF
            Table table = new Table(4); // 4 cột

            // Thêm tiêu đề cột
            table.AddHeaderCell("STT");
            table.AddHeaderCell("Họ và Tên");
            table.AddHeaderCell("Email");
            table.AddHeaderCell("Tình Trạng");

            // Thêm dữ liệu vào bảng
            foreach (var nhanVien in danhSachNhanVien)
            {
                table.AddCell(nhanVien.STT.ToString());
                table.AddCell(nhanVien.HoVaTen);
                table.AddCell(nhanVien.Email);
                table.AddCell(nhanVien.TinhTrang);
            }

            // Thêm bảng vào tài liệu
            document.Add(table);
            document.Close();

            // Trả về file PDF cho client
            return File(stream.ToArray(), "application/pdf", "DanhSachNhanVien.pdf");
        }

        public IActionResult Index()
        {
            // Lấy số lượng phòng
            int soLuongPhong = _context.Phongs.Count();

            // Lấy tổng số nhân viên
            int tongNhanVien = _context.Users.Count();

            // Truyền dữ liệu sang View
            ViewData["SoLuongPhong"] = soLuongPhong;
            ViewData["TongNhanVien"] = tongNhanVien;

            return View();
        }

        public IActionResult Phong()
        {
            var danhSachPhong = _context.Phongs
                .Select(p => new PhongViewModel
                {
                    PhongId = p.PhongId,
                    TenPhong = p.TenPhong ?? "Tên phòng chưa được đặt", // Kiểm tra NULL
                    MoTa = string.IsNullOrEmpty(p.MoTa) ? "Không có mô tả" : p.MoTa, // Kiểm tra NULL hoặc chuỗi rỗng
                    SoLuongNguoi = _context.Users.Count(u => u.PhongId == p.PhongId),
                    DanhSachNhanVien = _context.Users
                        .Where(u => u.PhongId == p.PhongId)
                        .Select(u => new UserViewModel
                        {
                            HoVaTen = string.IsNullOrEmpty(u.FullName) ? "Chưa có tên" : u.FullName, // Kiểm tra NULL
                            Email = u.Email ?? "Không có email" // Kiểm tra NULL
                        })
                        .ToList()
                })
                .ToList();

            return View(danhSachPhong);
        }






        public IActionResult NhanVien()
        {
            // Lấy danh sách nhân viên và chuyển sang dạng AsEnumerable() để xử lý phía client
            var danhSachNhanVien = _context.Users
                .AsEnumerable() // Thực hiện truy vấn trước khi xử lý trên client
                .Select((u, index) => new
                {
                    STT = index + 1,
                    HoVaTen = u.FullName,
                    Email = u.Email,
                    TinhTrang = u.Role == 0 ? "Admin" : "User"
                })
                .ToList();

            return View(danhSachNhanVien);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
