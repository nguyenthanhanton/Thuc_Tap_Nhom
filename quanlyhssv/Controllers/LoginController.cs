using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;

namespace quanlyhssv.Controllers
{
    public class LoginController : Controller
    {
        private readonly QuanlyhocsinhThptContext _context;

        public LoginController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Index(string user, string password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập và Mật khẩu";
                return View();
            }

            var taikhoan = await _context.Taikhoans
                .FirstOrDefaultAsync(t => t.Tendangnhap == user && t.Matkhau == password);

            if (taikhoan != null)
            {
                
                HttpContext.Session.SetString("Tendangnhap", taikhoan.Hoten ?? "");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }

       
        public IActionResult
            Logout()
        {
            // Xóa session hoặc cookie nếu cần
            HttpContext.Session.Remove("Tendangnhap");
            return RedirectToAction("Index", "Login");
        }
    }
}
