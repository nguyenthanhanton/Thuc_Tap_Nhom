
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;
namespace quanlyhssv.Controllers
{
    public class MonhocController : Controller
    {
        readonly private QuanlyhocsinhThptContext _context;
        public MonhocController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = await _context.Monhocs.CountAsync();
            var taikhoans = await _context.Monhocs
                .OrderBy(t => t.Mamon) // Sắp xếp theo mã
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
            var lastNumber = _context.Monhocs
                .AsEnumerable()
                .Where(t => !string.IsNullOrEmpty(t.Mamon) && t.Mamon.StartsWith("MH"))
                .Select(t => int.Parse(t.Mamon.Substring(2)))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            int nextNumber = lastNumber + 1;

            string newId = "MH" + nextNumber;

            // Tạo model đúng kiểu Taikhoan
            var model = new Monhoc
            {
                Mamon = newId
            };

            return View(model);   // ✔ đúng
        }
        [HttpPost]
        public IActionResult Create(Monhoc monhoc)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                bool isExist = _context.Monhocs
                                .Any(t => t.Tenmon == monhoc.Tenmon);
                if (isExist)
                {
                    // Thêm thông báo lỗi cho ModelState
                    ModelState.AddModelError("Tenmon", "Tên Môn đã tồn tại.");
                    return View(monhoc);
                }
                _context.Monhocs.Add(monhoc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monhoc);
        }
        public IActionResult Edit(string id)
        {
            var taikhoan = _context.Monhocs.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Monhoc taikhoan)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                bool isExist = _context.Monhocs
                                .Any(t => t.Tenmon == taikhoan.Tenmon);
                if (isExist)
                {
                    // Thêm thông báo lỗi cho ModelState
                    ModelState.AddModelError("Tenmon", "Tên Môn đã tồn tại.");
                    return View(taikhoan);
                }
                try
                {
                    // Cập nhật tài khoản
                    _context.Monhocs.Update(taikhoan);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Monhocs.Any(e => e.Mamon == taikhoan.Mamon))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }
            return View(taikhoan);
        }
        public async Task<IActionResult> Delete(string id)
        {
            // Kiểm tra môn học có tồn tại không trước
            var monhoc = await _context.Monhocs.FindAsync(id);
            if (monhoc == null)
            {
                return NotFound();
            }

            // Sử dụng Transaction để đảm bảo toàn vẹn dữ liệu
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Xóa phân công giảng dạy (Giangday) liên quan đến môn này
                    var giangdays = await _context.Giangdays
                                                  .Where(gd => gd.Mamon == id)
                                                  .ToListAsync();
                    if (giangdays.Any())
                    {
                        _context.Giangdays.RemoveRange(giangdays);
                    }

                    // 2. Xóa tất cả điểm số (Diem) liên quan đến môn này
                    // Lưu ý: Phải dùng Where để tìm theo Mamon, không dùng Find
                    var diems = await _context.Diems
                                              .Where(d => d.Mamon == id)
                                              .ToListAsync();
                    if (diems.Any())
                    {
                        _context.Diems.RemoveRange(diems);
                    }

                    // 3. Cập nhật Giáo viên đang phụ trách môn này (Set về MH0 hoặc null)
                    var giaoviens = await _context.Giaoviens
                                                  .Where(g => g.Mamon == id)
                                                  .ToListAsync();
                    if (giaoviens.Any())
                    {
                        foreach (var gv in giaoviens)
                        {
                            gv.Mamon = "MH0"; // Hoặc gv.Mamon = null; nếu DB cho phép null
                        }
                        _context.Giaoviens.UpdateRange(giaoviens);
                    }

                    // 4. Cuối cùng mới xóa Môn học
                    _context.Monhocs.Remove(monhoc);

                    // 5. Lưu tất cả thay đổi cùng lúc
                    await _context.SaveChangesAsync();

                    // Xác nhận giao dịch thành công
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Nếu có lỗi, hoàn tác mọi thứ để không bị mất dữ liệu dang dở
                    await transaction.RollbackAsync();
                    return BadRequest("Không thể xóa môn học này vì có lỗi hệ thống.");
                }
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Lấy thông tin môn học KÈM THEO danh sách giáo viên (quan trọng: .Include)
            var monhoc = await _context.Monhocs
                .Include(m => m.Giaoviens) // Load dữ liệu bảng Giaovien theo khóa ngoại
                .FirstOrDefaultAsync(m => m.Mamon == id);

            if (monhoc == null)
            {
                return NotFound();
            }

            return View(monhoc);
        }

    }
}
