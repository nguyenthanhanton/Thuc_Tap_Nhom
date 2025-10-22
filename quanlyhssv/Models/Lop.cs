using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Lop
{
    public string Malop { get; set; } = null!;

    public string Tenlop { get; set; } = null!;

    public string? Magv { get; set; }

    public string? Khoi { get; set; }

    public string? Manh { get; set; }

    public virtual ICollection<Giangday> Giangdays { get; set; } = new List<Giangday>();

    public virtual ICollection<Hocsinh> Hocsinhs { get; set; } = new List<Hocsinh>();

    public virtual Giaovien? MagvNavigation { get; set; }

    public virtual Namhoc? ManhNavigation { get; set; }
}
