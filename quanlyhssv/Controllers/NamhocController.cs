using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;


namespace quanlyhssv.Controllers
{
    public class NamhocController : Controller
    {
        private readonly QuanlyhocsinhThptContext _context;

        public NamhocController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }
        public IActionResult Edit(string id)
        {
            var taikhoan = _context.Namhocs.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Namhoc taikhoan)
        //{
        //    if (ModelState.IsValid)
        //    {
                
        //        try
        //        {
        //            // Cập nhật tài khoản
        //            _context.Namhocs.Update(taikhoan);
        //            _context.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!_context.Namhocs.Any(e => e.Manh == taikhoan.Manh))
        //                return NotFound();
        //            else
        //                throw;
        //        }

        //        return RedirectToAction("Index", "Home");

        //    }
        //    return View(taikhoan);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Namhoc taikhoan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Namhocs.Update(taikhoan);
                    _context.SaveChanges();

                    // Thông báo thành công
                    TempData["Success"] = "Cập nhật năm học thành công!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Thông báo lỗi từ Database (ví dụ: trùng mã, lỗi kết nối...)
                    TempData["Error"] = "Lỗi Database: " + ex.Message;
                }
            }
            else
            {
                // Nếu ModelState không hợp lệ, liệt kê các lỗi vào TempData
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["Error"] = "Dữ liệu không hợp lệ: " + errors;
            }

            return View(taikhoan);
        }
        public IActionResult Create()
        {
            // Lấy danh sách mã hiện có
            var lastNumber = _context.Namhocs
                .AsEnumerable()
                .Where(t => !string.IsNullOrEmpty(t.Manh) && t.Manh.StartsWith("NH"))
                .Select(t => int.Parse(t.Manh.Substring(2)))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            int nextNumber = lastNumber + 1;

            string newId = "NH" + nextNumber;

            // Tạo model đúng kiểu Taikhoan
            var model = new Namhoc
            {
                Manh = newId
            };

            return View(model);   // ✔ đúng
        }
        [HttpPost]
        public IActionResult Create(Namhoc namhoc)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                bool isExist = _context.Namhocs
                                .Any(t => t.Ten == namhoc.Ten);
                if (isExist)
                {
                    // Thêm thông báo lỗi cho ModelState
                    ModelState.AddModelError("Ten", "Tên năm học đã tồn tại.");
                    return View(namhoc);
                }
                
                string Mahk1 = "HK1-" + namhoc.Manh;
                string Mahk2 = "HK2-" + namhoc.Manh;
                Hocky hocky1 = new Hocky
                {
                    Mahk = Mahk1,
                    Ten= "1",
                    Manh = namhoc.Manh
                };
                Hocky hocky2 = new Hocky
                {
                    Mahk = Mahk2,
                    Ten = "2",
                    Manh = namhoc.Manh
                };
                _context.Namhocs.Add(namhoc);
                _context.Hockies.Add(hocky1);
                _context.Hockies.Add(hocky2);
                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(namhoc);
        }
    }
}
