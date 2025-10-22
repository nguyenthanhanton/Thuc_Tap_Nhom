using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Hocky
{
    public string Mahk { get; set; } = null!;

    public string? Manh { get; set; }

    public string? Ten { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual Namhoc? ManhNavigation { get; set; }
}
