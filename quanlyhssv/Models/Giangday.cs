using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Giangday
{
    public string Magv { get; set; } = null!;

    public string Malop { get; set; } = null!;

    public string Mamon { get; set; } = null!;

    public string Namhoc { get; set; } = null!;
    public string Mahk { get; set; } = null!;

    public virtual Giaovien MagvNavigation { get; set; } = null!;

    public virtual Lop MalopNavigation { get; set; } = null!;

    public virtual Monhoc MamonNavigation { get; set; } = null!;

    public virtual Namhoc NamhocNavigation { get; set; } = null!;
}
