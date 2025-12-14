//using quanlyhssv.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;

//namespace quanlyhssv.Controllers
//{
//    public class HocsinhController : Controller
//    {


//        private readonly QuanlyhocsinhThptContext _context;


//        public HocsinhController(QuanlyhocsinhThptContext context)
//        {
//            _context = context;
//        }
//        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string Manh = "0")
//        {
//            var query = _context.Hocsinhs.AsQueryable();

//            // Nếu có chọn năm học thì lọc
//            if (Manh != "0")
//            {
//                query = query.Where(h => h.Manh == Manh);
//            }

//            // Đếm số bản ghi sau khi lọc
//            var totalRecords = await query.CountAsync();

//            // Phân trang
//            var students = await query
//                .OrderBy(t => t.Mahs)
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            ViewBag.CurrentPage = page;
//            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
//            ViewBag.SelectedYear = Manh;   // để view nhận biết lựa chọn hiện tại

//            return View(students);
//        }
//        public IActionResult Create()
//        {
//            // Lấy danh sách mã hiện có
//            var lastNumber = _context.Hocsinhs
//                .AsEnumerable()
//                .Where(t => !string.IsNullOrEmpty(t.Mahs) && t.Mahs.StartsWith("HS"))
//                .Select(t => int.Parse(t.Mahs.Substring(2)))
//                .OrderByDescending(x => x)
//                .FirstOrDefault();

//            int nextNumber = lastNumber + 1;

//            string newId = "HS" + nextNumber;
//            var namhoc = _context.Namhocs.OrderByDescending(x=>x).FirstOrDefault();



//            // Tạo model đúng kiểu Taikhoan
//            var model = new Hocsinh
//            {
//                Mahs = newId,
//                Manh= namhoc.Manh

//            };

//            return View(model);   // ✔ đúng
//        }
//        [HttpPost]
//        public IActionResult Create(Hocsinh hocsinh)
//        {
//            if (hocsinh.Malop == "không") hocsinh.Malop = null;
//            if (ModelState.IsValid)
//            {

//                if (hocsinh.Hotenhs==null)
//                {
//                    // Thêm thông báo lỗi cho ModelState
//                    ModelState.AddModelError("Hotenhs", "Nhập tên đi.");
//                    return View(hocsinh);
//                }
//                _context.Hocsinhs.Add(hocsinh);
//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(hocsinh);
//        }
//        public IActionResult Edit(string id)
//        {
//            var taikhoan = _context.Hocsinhs.Find(id);
//            if (taikhoan == null)
//            {
//                return NotFound();
//            }
//            return View(taikhoan);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(Hocsinh taikhoan)
//        {
//            if (ModelState.IsValid)
//            {
//                if(taikhoan.Malop=="không") taikhoan.Malop = null;
//                try
//                {
//                    // Cập nhật tài khoản
//                    _context.Hocsinhs.Update(taikhoan);
//                    _context.SaveChanges();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Hocsinhs.Any(e => e.Mahs == taikhoan.Mahs))
//                        return NotFound();
//                    else
//                        throw;
//                }

//                return RedirectToAction("Index");
//            }
//            return View(taikhoan);
//        }
//        public IActionResult Delete(string id)
//        {
//            var diem = _context.Diems.Find(id);
//            if (diem != null)
//            {
//                _context.Diems.Remove(diem);

//            }
//            var taikhoan = _context.Hocsinhs.Find(id);
//            if (taikhoan == null)
//            {
//                return NotFound();
//            }

//            _context.Hocsinhs.Remove(taikhoan);
//            _context.SaveChanges();
//            return RedirectToAction("Index");
//        }


//    }
//}

using quanlyhssv.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Cần thêm để sử dụng LINQ

namespace quanlyhssv.Controllers
{
    public class HocsinhController : Controller
    {
        private readonly QuanlyhocsinhThptContext _context;

        public HocsinhController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string Manh = "0")
        {
            var query = _context.Hocsinhs.AsQueryable();

            if (Manh != "0")
            {
                query = query.Where(h => h.Manh == Manh);
            }

            var totalRecords = await query.CountAsync();

            var students = await query
                .OrderBy(t => t.Mahs)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SelectedYear = Manh;

            return View(students);
        }

        public IActionResult Create()
        {
            // Logic tạo mã HS tự động (giữ nguyên code của bạn)
            var lastNumber = _context.Hocsinhs
                .AsEnumerable()
                .Where(t => !string.IsNullOrEmpty(t.Mahs) && t.Mahs.StartsWith("HS"))
                .Select(t => int.Parse(t.Mahs.Substring(2)))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            int nextNumber = lastNumber + 1;
            string newId = "HS" + nextNumber;
            var namhoc = _context.Namhocs.OrderByDescending(x => x.Manh).FirstOrDefault();

            var model = new Hocsinh
            {
                Mahs = newId,
                Manh = namhoc?.Manh
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hocsinh hocsinh)
        {
            // Xử lý giá trị "không" từ dropdown (nếu có)
            if (hocsinh.Malop == "không") hocsinh.Malop = null;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(hocsinh.Hotenhs))
                {
                    ModelState.AddModelError("Hotenhs", "Nhập tên đi.");
                    return View(hocsinh);
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Lưu học sinh
                        _context.Hocsinhs.Add(hocsinh);
                        _context.SaveChanges();

                        // 2. Tạo điểm tự động nếu có lớp
                        if (!string.IsNullOrEmpty(hocsinh.Malop))
                        {
                            TaoDiemTuDong(hocsinh.Mahs, hocsinh.Malop);
                        }

                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Lỗi khi thêm học sinh: " + ex.Message);
                    }
                }
            }
            return View(hocsinh);
        }

        public IActionResult Edit(string id)
        {
            var taikhoan = _context.Hocsinhs.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hocsinh taikhoan)
        {
            if (ModelState.IsValid)
            {
                if (taikhoan.Malop == "không") taikhoan.Malop = null;

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Lấy thông tin cũ để kiểm tra xem lớp có đổi không
                        // Dùng AsNoTracking để không bị conflict với lệnh Update phía dưới
                        var hocSinhCu = _context.Hocsinhs.AsNoTracking()
                                                .FirstOrDefault(x => x.Mahs == taikhoan.Mahs);

                        if (hocSinhCu == null) return NotFound();

                        string lopCu = hocSinhCu.Malop;
                        string lopMoi = taikhoan.Malop;

                        // 2. Cập nhật thông tin học sinh
                        _context.Hocsinhs.Update(taikhoan);
                        _context.SaveChanges();

                        // 3. Kiểm tra logic đổi lớp
                        // Nếu lớp thay đổi (khác null và khác lớp cũ)
                        if (lopCu != lopMoi)
                        {
                            // A. Xóa toàn bộ điểm cũ của học sinh này
                            var diemCu = _context.Diems.Where(d => d.Mahs == taikhoan.Mahs).ToList();
                            if (diemCu.Any())
                            {
                                _context.Diems.RemoveRange(diemCu);
                                _context.SaveChanges();
                            }

                            // B. Nếu lớp mới không null, tạo điểm mới
                            if (!string.IsNullOrEmpty(lopMoi))
                            {
                                TaoDiemTuDong(taikhoan.Mahs, lopMoi);
                            }
                        }

                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        transaction.Rollback();
                        if (!_context.Hocsinhs.Any(e => e.Mahs == taikhoan.Mahs))
                            return NotFound();
                        else
                            throw;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                    }
                }
            }
            return View(taikhoan);
        }

        public IActionResult Delete(string id)
        {
            // Tìm học sinh
            var taikhoan = _context.Hocsinhs.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Tìm và xóa tất cả điểm của học sinh này trước
                    var dsDiem = _context.Diems.Where(d => d.Mahs == id).ToList();
                    if (dsDiem.Count > 0)
                    {
                        _context.Diems.RemoveRange(dsDiem);
                    }

                    // 2. Xóa học sinh
                    _context.Hocsinhs.Remove(taikhoan);

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    // Có thể return View báo lỗi hoặc redirect
                    return BadRequest("Không thể xóa học sinh này do lỗi hệ thống.");
                }
            }

            return RedirectToAction("Index");
        }

        // --- HÀM HỖ TRỢ: TẠO ĐIỂM TỰ ĐỘNG ---
        private void TaoDiemTuDong(string maHs, string maLop)
        {
            // 1. Tìm xem lớp này được phân công dạy những môn nào, học kỳ nào
            // Bảng Giangday liên kết Lop -> Monhoc & Hocky
            var danhSachPhanCong = _context.Giangdays
                                           .Where(gd => gd.Malop == maLop)
                                           .Select(gd => new { gd.Mamon, gd.Mahk })
                                           .Distinct() // Tránh trùng lặp nếu có
                                           .ToList();

            // 2. Tạo danh sách điểm cần thêm
            var listDiemMoi = new List<Diem>();

            foreach (var item in danhSachPhanCong)
            {
                // Kiểm tra xem đã tồn tại điểm chưa (đề phòng duplicate)
                bool daCoDiem = _context.Diems.Any(d => d.Mahs == maHs &&
                                                        d.Mamon == item.Mamon &&
                                                        d.Mahocky == item.Mahk);
                if (!daCoDiem)
                {
                    var diemMoi = new Diem
                    {
                        // Tạo Madiem tự động (dùng GUID cắt ngắn hoặc logic riêng của bạn)
                        Madiem = Guid.NewGuid().ToString().Substring(0, 10),
                        Mahs = maHs,
                        Mamon = item.Mamon,
                        Mahocky = item.Mahk,
                        // Các điểm thành phần để null hoặc 0 tùy yêu cầu
                        Diemmieng = null,
                        Diem15p = null,
                        DiemTh = null,
                        Diemhs2 = null,
                        Diemhs3 = null,
                        Diemtbmon = null
                    };
                    listDiemMoi.Add(diemMoi);
                }
            }

            // 3. Lưu vào DB
            if (listDiemMoi.Count > 0)
            {
                _context.Diems.AddRange(listDiemMoi);
                _context.SaveChanges();
            }
        }
    }
}