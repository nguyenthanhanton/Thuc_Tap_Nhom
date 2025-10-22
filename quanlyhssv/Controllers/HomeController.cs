using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlyhssv.Models;
using quanlyhssv.Models.ViewsModel;
using System.Diagnostics;

namespace quanlyhssv.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        readonly private QuanlyhocsinhThptContext _context;
   
        public HomeController(ILogger<HomeController> logger, QuanlyhocsinhThptContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tongHocSinh = await _context.Hocsinhs.CountAsync();
            var tongLop = await _context.Lops.CountAsync();
            var tongGiaoVien = await _context.Giaoviens.CountAsync();

            var danhSachNamHoc = await _context.Namhocs.ToListAsync();
            var thongKeTheoNam = new List<NamhocStat>();

            foreach (var n in danhSachNamHoc)
            {
                thongKeTheoNam.Add(new NamhocStat
                {
                    TenNamHoc = n.Ten,
                    Manh = n.Manh,
                    SoLop = await _context.Lops.CountAsync(l => l.Manh == n.Manh),
                    SoHocSinh = await _context.Hocsinhs.CountAsync(h => h.Manh == n.Manh)
                });
            }

            var viewModel = new DashboardViewModel
            {
                TongHocSinh = tongHocSinh,
                TongLop = tongLop,
                TongGiaoVien = tongGiaoVien,
                ThongKeTheoNam = thongKeTheoNam
            };

            return View(viewModel);
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
