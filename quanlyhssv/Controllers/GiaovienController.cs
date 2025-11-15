using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;
namespace quanlyhssv.Controllers
{
    public class GiaovienController : Controller
    {
        readonly private QuanlyhocsinhThptContext _context;
        public GiaovienController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = await _context.Giaoviens.CountAsync();
            var taikhoans = await _context.Giaoviens
                .OrderBy(t => t.Magv) // Sắp xếp theo mã
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
            var lastNumber = _context.Giaoviens
                .AsEnumerable()
                .Where(t => !string.IsNullOrEmpty(t.Magv) && t.Magv.StartsWith("GV"))
                .Select(t => int.Parse(t.Magv.Substring(2)))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            int nextNumber = lastNumber + 1;

            string newId = "GV" + nextNumber;

            // Tạo model đúng kiểu Taikhoan
            var model = new Giaovien
            {
                Magv = newId
            };

            return View(model);   // ✔ đúng
        }
        [HttpPost]
        public IActionResult Create(Giaovien giaovien)
        {
            if (ModelState.IsValid)
            {
                _context.Giaoviens.Add(giaovien);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(giaovien);
        }
        public IActionResult Edit(string id)
        {
            var taikhoan = _context.Giaoviens.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Giaovien taikhoan)
        {
            if (ModelState.IsValid)
            {


                try
                {
                    // Cập nhật tài khoản
                    _context.Giaoviens.Update(taikhoan);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Giaoviens.Any(e => e.Magv == taikhoan.Magv))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }

            return View(taikhoan);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var lop = _context.Lops.Where(l =>l.Magv==id);
            foreach (var l in lop)
            {
                l.Magv = null;
                _context.Lops.Update(l);
            }
            var giangday= _context.Giangdays.Where(l =>l.Magv==id);
            foreach (var l in giangday)
            {
                _context.Giangdays.Remove(l);
            }
            var taikhoan = _context.Giaoviens.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            _context.Giaoviens.Remove(taikhoan);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
