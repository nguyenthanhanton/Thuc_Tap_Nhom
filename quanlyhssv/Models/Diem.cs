using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Diem
{
    public string Madiem { get; set; } = null!;

    public string Mahs { get; set; } = null!;

    public string Mamon { get; set; } = null!;

    public string Mahocky { get; set; } = null!;

    public double? Diemmieng { get; set; }

    public double? Diem15p { get; set; }

    public double? DiemTh { get; set; }

    public double? Diemhs2 { get; set; }

    public double? Diemhs3 { get; set; }

    public double? Diemtbmon { get; set; }

    public virtual Hocky MahockyNavigation { get; set; } = null!;

    public virtual Hocsinh MahsNavigation { get; set; } = null!;

    public virtual Monhoc MamonNavigation { get; set; } = null!;
}
