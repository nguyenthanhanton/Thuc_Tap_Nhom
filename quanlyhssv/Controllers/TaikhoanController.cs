using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;
namespace quanlyhssv.Controllers
{
    
    public class TaikhoanController : Controller
    {
        readonly private QuanlyhocsinhThptContext _context;
        public TaikhoanController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
{
    var totalRecords = await _context.Taikhoans.CountAsync();
    var taikhoans = await _context.Taikhoans
        .OrderBy(t => t.Matk) // Sắp xếp theo mã
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    return View(taikhoans);
}

        public IActionResult Create()
        {
            // Lấy danh sách mã hiện có
            var lastNumber = _context.Taikhoans
                .AsEnumerable()
                .Where(t => !string.IsNullOrEmpty(t.Matk) && t.Matk.StartsWith("TK"))
                .Select(t => int.Parse(t.Matk.Substring(2)))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            int nextNumber = lastNumber + 1;

            string newId = "TK" + nextNumber;

            // Tạo model đúng kiểu Taikhoan
            var model = new Taikhoan
            {
                Matk = newId
            };

            return View(model);   // ✔ đúng
        }

        public IActionResult Edit(string id)
        {
            var taikhoan = _context.Taikhoans.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Taikhoan taikhoan)
        {
            if (ModelState.IsValid)
            {
             

                try
                {
                    // Cập nhật tài khoản
                    _context.Taikhoans.Update(taikhoan);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Taikhoans.Any(e => e.Matk == taikhoan.Matk))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }

            return View(taikhoan);
        }

        [HttpPost]
        public IActionResult Create(Taikhoan taikhoan)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                bool isExist = _context.Taikhoans
                                .Any(t => t.Tendangnhap == taikhoan.Tendangnhap);
                if (isExist)
                {
                    // Thêm thông báo lỗi cho ModelState
                    ModelState.AddModelError("Tendangnhap", "Tên đăng nhập đã tồn tại.");
                    return View(taikhoan);
                }

                if (taikhoan.Quyenhan == "1")
                {
                    ModelState.AddModelError("Quyenhan", "Vui lòng chọn quyền hạn.");
                    return View(taikhoan);
                }
                // Nếu chưa tồn tại, thêm mới
                _context.Taikhoans.Add(taikhoan);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taikhoan);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var taikhoan = _context.Taikhoans.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            _context.Taikhoans.Remove(taikhoan);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
