using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Namhoc
{
    public string Manh { get; set; } = null!;

    public string? Ten { get; set; }

    public virtual ICollection<Giangday> Giangdays { get; set; } = new List<Giangday>();

    public virtual ICollection<Hocky> Hockies { get; set; } = new List<Hocky>();

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
    //public virtual ICollection<Hocsinh> Hocsinhs { get; set; }
}
