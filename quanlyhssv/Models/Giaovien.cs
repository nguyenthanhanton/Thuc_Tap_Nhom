using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Giaovien
{
    public string Magv { get; set; } = null!;

    public string Hotengv { get; set; } = null!;

    public DateOnly? Ngaysinh { get; set; }

    public string? Gioitinh { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? Mamon { get; set; }

    public virtual ICollection<Giangday> Giangdays { get; set; } = new List<Giangday>();

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();

    public virtual Monhoc? MamonNavigation { get; set; }
}
