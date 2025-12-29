namespace quanlyhssv.Models.ViewsModel
{
    public class NhapDiemViewModel
    {
        public string? Malop { get; set; }
        public string? Tenlop { get; set; }
        public string? Mamon { get; set; }
        public string? Tenmon { get; set; }
        public string? Mahk { get; set; }
        public List<HocSinhDiemItem> DanhSachDiem { get; set; } = new List<HocSinhDiemItem>();
    }
}
