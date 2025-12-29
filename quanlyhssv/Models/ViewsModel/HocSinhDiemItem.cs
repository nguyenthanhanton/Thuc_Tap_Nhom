using System.ComponentModel.DataAnnotations;

namespace quanlyhssv.Models.ViewsModel
{
    public class HocSinhDiemItem
    {
        public string? Mahs { get; set; }
        public string? Hoten { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0-10")]
        public double? Diemmieng { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0-10")]
        public double? Diem15p { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0-10")]
        public double? DiemTh { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0-10")]
        public double? Diemhs2 { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0-10")]
        public double? Diemhs3 { get; set; }

        public double? Diemtbmon { get; set; }
    }
}
