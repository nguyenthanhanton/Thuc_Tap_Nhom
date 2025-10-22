using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Hocsinh
{
    public string Mahs { get; set; } = null!;

    public string? Hotenhs { get; set; } = null!;

    public string? Diachi { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public string? Gioitinh { get; set; }

    public string? Malop { get; set; }

    public string? Manh { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual Lop? MalopNavigation { get; set; }
}
