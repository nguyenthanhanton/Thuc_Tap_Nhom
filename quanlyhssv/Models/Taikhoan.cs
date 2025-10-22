using System;
using System.Collections.Generic;

namespace quanlyhssv.Models;

public partial class Taikhoan
{
    public string Matk { get; set; } = null!;

    public string? Tendangnhap { get; set; }

    public string? Matkhau { get; set; }

    public string? Quyenhan { get; set; }

    public string? Hoten { get; set; }
}
