using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Monhoc
{
    public string Mamon { get; set; } = null!;

    public string? Tenmon { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<Giangday> Giangdays { get; set; } = new List<Giangday>();

    public virtual ICollection<Giaovien> Giaoviens { get; set; } = new List<Giaovien>();
}
