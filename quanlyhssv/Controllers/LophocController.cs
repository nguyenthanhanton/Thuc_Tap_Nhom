using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;
using quanlyhssv.Models.ViewsModel;
using System.Diagnostics;
namespace quanlyhssv.Controllers
{
    public class LophocController : Controller
    {

        private readonly QuanlyhocsinhThptContext _context;
        public LophocController(QuanlyhocsinhThptContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string Manh = "0")
        {
            var query = _context.Lops.AsQueryable();

            // Nếu có chọn năm học thì lọc
            if (Manh != "0")
            {
                query = query.Where(h => h.Manh == Manh);
            }

            // Đếm số bản ghi sau khi lọc
            var totalRecords = await query.CountAsync();

            // Phân trang
            var lop = await query
                .OrderBy(t => t.Malop)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SelectedYear = Manh;   // để view nhận biết lựa chọn hiện tại

            return View(lop);
        }
        public IActionResult Create()
        {
            // Lấy danh sách mã hiện có
            var lastNumber = _context.Lops
                .AsEnumerable()
                .Where(t => !string.IsNullOrEmpty(t.Malop) && t.Malop.StartsWith("L"))
                .Select(t => int.Parse(t.Malop.Substring(1)))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            int nextNumber = lastNumber + 1;

            string newId = "L" + nextNumber;
            var namhoc = _context.Namhocs.OrderByDescending(x => x).FirstOrDefault();



            // Tạo model đúng kiểu Taikhoan
            var model = new Lop
            {
                Malop = newId,
                Manh = namhoc.Manh

            };

            return View(model);   // ✔ đúng
        }
        [HttpPost]
        public IActionResult Create(Lop lop)
        {
            if (lop.Magv == "không") lop.Magv = null;
            if (ModelState.IsValid)
            {
                _context.Lops.Add(lop);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lop);
        }
        public IActionResult Edit(string id)
        {
            var taikhoan = _context.Lops.Find(id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Lop taikhoan)
        {
            if (ModelState.IsValid)
            {
                if (taikhoan.Magv == "không") taikhoan.Magv = null;
                try
                {
                    // Cập nhật tài khoản
                    _context.Lops.Update(taikhoan);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Lops.Any(e => e.Malop == taikhoan.Malop))
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
            // 1. Tìm lớp học cần xóa
            var lop = _context.Lops.Find(id);
            if (lop == null)
            {
                return NotFound();
            }

            try
            {
                // 2. Xử lý học sinh và điểm của học sinh trong lớp này
                var danhSachHocSinh = _context.Hocsinhs.Where(h => h.Malop == id).ToList();
                var studentIds = danhSachHocSinh.Select(h => h.Mahs).ToList();

                // Xóa toàn bộ điểm của những học sinh thuộc lớp này
                var diemsToRemove = _context.Diems.Where(d => studentIds.Contains(d.Mahs)).ToList();
                if (diemsToRemove.Any())
                {
                    _context.Diems.RemoveRange(diemsToRemove);
                }

                // Cập nhật học sinh: Rời khỏi lớp (Gán Malop = null) 
                // Hoặc nếu bạn muốn XÓA luôn học sinh thì dùng _context.Hocsinhs.RemoveRange(danhSachHocSinh);
                foreach (var hs in danhSachHocSinh)
                {
                    hs.Malop = null;
                }

                // 3. Xóa toàn bộ phân công giảng dạy của lớp này
                var giangDaysToRemove = _context.Giangdays.Where(g => g.Malop == id).ToList();
                if (giangDaysToRemove.Any())
                {
                    _context.Giangdays.RemoveRange(giangDaysToRemove);
                }

                // 4. Cuối cùng mới xóa Lớp học
                _context.Lops.Remove(lop);

                // Lưu tất cả thay đổi vào DB trong 1 lần duy nhất
                _context.SaveChanges();

                TempData["Success"] = "Đã xóa lớp học và toàn bộ dữ liệu liên quan thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> phutrach(int page = 1, int pageSize = 5, string Manh = "0")
        {
            var query = _context.Lops.AsQueryable();

            // Nếu có chọn năm học thì lọc
            if (Manh != "0")
            {
                query = query.Where(h => h.Manh == Manh);
            }

            // Đếm số bản ghi sau khi lọc
            var totalRecords = await query.CountAsync();

            // Phân trang
            var lop = await query
                .OrderBy(t => t.Malop)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SelectedYear = Manh;   // để view nhận biết lựa chọn hiện tại

            return View(lop);
        }
        [HttpPost]
        public IActionResult Phutrach()
        {
            var form = HttpContext.Request.Form;

            foreach (var key in form.Keys)
            {
                if (key.StartsWith("magv_"))
                {
                    var malop = key.Replace("magv_", "");
                    var magvValue = form[key];
                    var magv = magvValue.ToString();

                    var lop = _context.Lops.FirstOrDefault(l => l.Malop == malop);
                    if (lop != null)
                    {
                        // Chỉ cần kiểm tra rỗng
                        lop.Magv = string.IsNullOrWhiteSpace(magv) ? null : magv;
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("phutrach");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            // 1. Lấy thông tin lớp và nạp tất cả các bảng liên quan (Eager Loading)
            var lop = await _context.Lops
                .Include(l => l.Hocsinhs)
                .Include(l => l.Giangdays)
                    .ThenInclude(g => g.MamonNavigation)
                .Include(l => l.Giangdays)
                    .ThenInclude(g => g.MagvNavigation)
                .FirstOrDefaultAsync(m => m.Malop == id);

            if (lop == null) return NotFound();

            // 2. Lấy danh sách học kỳ thuộc năm học của lớp này
            var danhSachHocKy = await _context.Hockies
                .Where(h => h.Manh == lop.Manh)
                .OrderBy(h => h.Ten)
                .ToListAsync();

            ViewBag.Hockies = danhSachHocKy;

            return View(lop);
        }
        public IActionResult AssignSubject(string malop)
        {
            var lop = _context.Lops.Find(malop);
            if (lop == null) return NotFound();

            ViewBag.Lop = lop;
            ViewBag.MonHoc = _context.Monhocs.ToList();

            // Không nạp toàn bộ giáo viên vào ViewBag nữa
            // Chúng ta sẽ nạp qua AJAX khi người dùng chọn môn học

            ViewBag.HocKy = _context.Hockies.Where(kh => kh.Manh == lop.Manh).ToList();
            return View();
        }

        // 2. Action API: Trả về danh sách giáo viên dạy môn học tương ứng (Dùng cho AJAX)
        [HttpGet]
        public async Task<JsonResult> GetGiaoVienByMonHoc(string mamon)
        {
            var giaoviens = await _context.Giaoviens
                .Where(g => g.Mamon == mamon)
                .Select(g => new
                {
                    magv = g.Magv,
                    hotengv = g.Hotengv
                })
                .ToListAsync();

            return Json(giaoviens);
        }

        // 3. Action POST: Xử lý lưu dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSubject(string malop, string mamon, string magv, string mahocky)
        {
            // Kiểm tra trùng lặp
            var isExisted = await _context.Giangdays
         .AnyAsync(g => g.Malop == malop && g.Mamon == mamon && g.Mahk == mahocky);

            if (isExisted)
            {
                TempData["Error"] = "Môn học này đã có trong lớp này!";
                return RedirectToAction("Details", new { id = malop });
            }

            // Thêm vào bảng giảng dạy
            var giangDay = new Giangday
            {
                Malop = malop,
                Mamon = mamon,
                Magv = magv,
                Namhoc = _context.Lops.Find(malop)?.Manh,
                Mahk = mahocky
            };
            _context.Giangdays.Add(giangDay);

            // Tạo bản ghi điểm cho học sinh
            var danhSachHocSinh = await _context.Hocsinhs
                .Where(h => h.Malop == malop)
                .ToListAsync();

            foreach (var hs in danhSachHocSinh)
            {
                _context.Diems.Add(new Diem
                {
                    Madiem = "D" + Guid.NewGuid().ToString().Substring(0, 8),
                    Mahs = hs.Mahs,
                    Mamon = mamon,
                    Mahocky = mahocky
                });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã thêm môn học và khởi tạo bảng điểm thành công!";
            return RedirectToAction("Details", new { id = malop });
        }
        [HttpPost]
        public IActionResult xoamonhoc(string malop, string mamon, string mahk)
        {
            // 1. Tìm bản ghi phân công giảng dạy tương ứng
            var giangDay = _context.Giangdays.FirstOrDefault(g =>
                g.Malop == malop &&
                g.Mamon == mamon &&
                g.Mahk == mahk);
            if (giangDay != null)
            {
                try
                {
                    // 2. Tìm danh sách điểm của tất cả học sinh thuộc lớp này trong môn học và học kỳ này
                    var diemsToRemove = _context.Diems.Where(d =>
                        d.Mamon == mamon &&
                        d.Mahocky == mahk &&
                        _context.Hocsinhs.Any(h => h.Mahs == d.Mahs && h.Malop == malop)
                    ).ToList();

                    // 3. Thực hiện xóa điểm trước (để tránh lỗi khóa ngoại)
                    if (diemsToRemove.Any())
                    {
                        _context.Diems.RemoveRange(diemsToRemove);
                    }

                    // 4. Thực hiện xóa phân công giảng dạy
                    _context.Giangdays.Remove(giangDay);

                    // 5. Lưu tất cả thay đổi vào Database
                    _context.SaveChanges();

                    TempData["Success"] = $"Đã xóa môn học và {diemsToRemove.Count} bản ghi điểm liên quan.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Có lỗi xảy ra trong quá trình xóa dữ liệu: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Không tìm thấy phân công giảng dạy để xóa.";
            }

            // 6. Quay lại trang chi tiết của lớp học
            return RedirectToAction("Details", new { id = malop });
        }
        [HttpGet]
        public IActionResult NhapDiem(string malop, string mamon, string mahk)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrEmpty(malop) || string.IsNullOrEmpty(mamon) || string.IsNullOrEmpty(mahk))
            {
                return NotFound("Thiếu thông tin mã lớp, mã môn hoặc mã học kỳ.");
            }

            var lop = _context.Lops.FirstOrDefault(l => l.Malop == malop);
            var mon = _context.Monhocs.FirstOrDefault(m => m.Mamon == mamon);

            if (lop == null || mon == null) return NotFound();

            // 2. Lấy danh sách học sinh
            var hocsinhs = _context.Hocsinhs
                                   .Where(h => h.Malop == malop)
                                   .OrderBy(h => h.Hotenhs)
                                   .ToList();

            // 3. Khởi tạo ViewModel
            var model = new NhapDiemViewModel
            {
                Malop = malop,
                Tenlop = lop.Tenlop,
                Mamon = mamon,
                Tenmon = mon.Tenmon,
                Mahk = mahk
            };

            // 4. Map dữ liệu (QUAN TRỌNG: Dùng Trim() để xử lý lỗi khoảng trắng database)
            foreach (var hs in hocsinhs)
            {
                // Tìm điểm hiện có trong DB
                var diem = _context.Diems.FirstOrDefault(d => d.Mahs == hs.Mahs
                                                           && d.Mamon.Trim() == mamon.Trim()
                                                           && d.Mahocky.Trim() == mahk.Trim());

                model.DanhSachDiem.Add(new HocSinhDiemItem
                {
                    Mahs = hs.Mahs,
                    Hoten = hs.Hotenhs,
                    // Load điểm cũ lên nếu có
                    Diemmieng = diem?.Diemmieng,
                    Diem15p = diem?.Diem15p,
                    DiemTh = diem?.DiemTh,
                    Diemhs2 = diem?.Diemhs2,
                    Diemhs3 = diem?.Diemhs3,
                    Diemtbmon = diem?.Diemtbmon
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LuuDiem(NhapDiemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // --- ĐOẠN XỬ LÝ LƯU GIỮ NGUYÊN ---
                    foreach (var item in model.DanhSachDiem)
                    {
                        // 1. Tính toán ĐTB
                        double dMieng = item.Diemmieng ?? 0;
                        double d15 = item.Diem15p ?? 0;
                        double dTh = item.DiemTh ?? 0;
                        double dHs2 = item.Diemhs2 ?? 0;
                        double dHs3 = item.Diemhs3 ?? 0;

                        double dtb = (dMieng * 0.1) + (d15 * 0.1) + (dTh * 0.1) + (dHs2 * 0.3) + (dHs3 * 0.4);

                        // 2. Tìm bản ghi (Dùng Trim để chính xác)
                        var diemEntity = await _context.Diems.FirstOrDefaultAsync(d =>
                                            d.Mahs == item.Mahs &&
                                            d.Mamon.Trim() == model.Mamon.Trim() &&
                                            d.Mahocky.Trim() == model.Mahk.Trim());

                        if (diemEntity == null)
                        {
                            // Thêm mới
                            diemEntity = new Diem();
                            diemEntity.Madiem = Guid.NewGuid().ToString().Substring(0, 10);
                            diemEntity.Mahs = item.Mahs;
                            diemEntity.Mamon = model.Mamon;
                            diemEntity.Mahocky = model.Mahk;
                            // Gán điểm...
                            diemEntity.Diemmieng = item.Diemmieng;
                            diemEntity.Diem15p = item.Diem15p;
                            diemEntity.DiemTh = item.DiemTh;
                            diemEntity.Diemhs2 = item.Diemhs2;
                            diemEntity.Diemhs3 = item.Diemhs3;
                            diemEntity.Diemtbmon = Math.Round(dtb, 2);
                            _context.Diems.Add(diemEntity);
                        }
                        else
                        {
                            // Cập nhật
                            diemEntity.Diemmieng = item.Diemmieng;
                            diemEntity.Diem15p = item.Diem15p;
                            diemEntity.DiemTh = item.DiemTh;
                            diemEntity.Diemhs2 = item.Diemhs2;
                            diemEntity.Diemhs3 = item.Diemhs3;
                            diemEntity.Diemtbmon = Math.Round(dtb, 2);
                            _context.Diems.Update(diemEntity);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Đã lưu bảng điểm thành công!";
                    return RedirectToAction("NhapDiem", new { malop = model.Malop, mamon = model.Mamon, mahk = model.Mahk });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi hệ thống (Try-Catch): " + ex.Message;
                }
            }
            else
            {
                // --- SỬA LỖI Ở ĐÂY: BẮT LỖI CHI TIẾT ---

                // 1. Lấy tất cả lỗi từ ModelState
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage) // Lấy thông báo lỗi
                    .ToList();

                // 2. Kiểm tra xem có lỗi chuyển đổi kiểu dữ liệu không (Ví dụ nhập chữ vào ô số)
                var exceptionErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => e.Exception != null)
                    .Select(e => "Lỗi định dạng: " + e.Exception.Message)
                    .ToList();

                // 3. Gộp lại thành 1 chuỗi
                var allErrors = string.Join(" | ", errors.Concat(exceptionErrors));

                // 4. In ra màn hình để bạn đọc
                TempData["Error"] = $"Dữ liệu không hợp lệ: {allErrors}";

                // Debug: In ra Output window của Visual Studio nếu cần
                System.Diagnostics.Debug.WriteLine("VALIDATION ERROR: " + allErrors);
            }

            return View("NhapDiem", model);
        }
    }
    }
