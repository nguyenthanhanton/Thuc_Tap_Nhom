using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace quanlyhssv.Models;

public partial class QuanlyhocsinhThptContext : DbContext
{
    public QuanlyhocsinhThptContext()
    {
    }

    public QuanlyhocsinhThptContext(DbContextOptions<QuanlyhocsinhThptContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diem> Diems { get; set; }

    public virtual DbSet<Giangday> Giangdays { get; set; }

    public virtual DbSet<Giaovien> Giaoviens { get; set; }

    public virtual DbSet<Hocky> Hockies { get; set; }

    public virtual DbSet<Hocsinh> Hocsinhs { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<Monhoc> Monhocs { get; set; }

    public virtual DbSet<Namhoc> Namhocs { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=ANTON;Initial Catalog=quanlyhocsinhTHPT;Encrypt=false;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.Madiem).HasName("PK__Diem__2D9468E34FFB399D");

            entity.ToTable("Diem");

            entity.Property(e => e.Madiem)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DiemTh).HasColumnName("DiemTH");
            entity.Property(e => e.Mahocky)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mahs)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mamon)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.MahockyNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.Mahocky)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diem_Hocky");

            entity.HasOne(d => d.MahsNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.Mahs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diem__Mahs__5DCAEF64");

            entity.HasOne(d => d.MamonNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.Mamon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diem__Mamon__5EBF139D");
        });

        modelBuilder.Entity<Giangday>(entity =>
        {
            entity.HasKey(e => new { e.Magv, e.Malop, e.Mamon, e.Namhoc }).HasName("PK__Giangday__9248CD93FCF90E8F");

            entity.ToTable("Giangday");

            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Malop)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mamon)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Namhoc)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.MagvNavigation).WithMany(p => p.Giangdays)
                .HasForeignKey(d => d.Magv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Giangday__Magv__628FA481");

            entity.HasOne(d => d.MalopNavigation).WithMany(p => p.Giangdays)
                .HasForeignKey(d => d.Malop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Giangday__Malop__6383C8BA");

            entity.HasOne(d => d.MamonNavigation).WithMany(p => p.Giangdays)
                .HasForeignKey(d => d.Mamon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Giangday__Mamon__6477ECF3");

            entity.HasOne(d => d.NamhocNavigation).WithMany(p => p.Giangdays)
                .HasForeignKey(d => d.Namhoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Giangday_Namhoc");
        });

        modelBuilder.Entity<Giaovien>(entity =>
        {
            entity.HasKey(e => e.Magv).HasName("PK__Giaovien__2724A2BB0CD9A7B6");

            entity.ToTable("Giaovien");

            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gioitinh).HasMaxLength(10);
            entity.Property(e => e.Hotengv).HasMaxLength(50);
            entity.Property(e => e.Mamon)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");

            entity.HasOne(d => d.MamonNavigation).WithMany(p => p.Giaoviens)
                .HasForeignKey(d => d.Mamon)
                .HasConstraintName("FK_Giaovien_Monhoc");
        });

        modelBuilder.Entity<Hocky>(entity =>
        {
            entity.HasKey(e => e.Mahk).HasName("PK__Hocky__27249AAF10118A56");

            entity.ToTable("Hocky");

            entity.Property(e => e.Mahk)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Manh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ManhNavigation).WithMany(p => p.Hockies)
                .HasForeignKey(d => d.Manh)
                .HasConstraintName("FK__Hocky__Ten__2EDAF651");
        });

        modelBuilder.Entity<Hocsinh>(entity =>
        {
            entity.HasKey(e => e.Mahs).HasName("PK__Hocsinh__27249A57A4C3D2A2");

            entity.ToTable("Hocsinh");

            entity.Property(e => e.Mahs)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Diachi).HasMaxLength(100);
            entity.Property(e => e.Gioitinh).HasMaxLength(10);
            entity.Property(e => e.Hotenhs).HasMaxLength(50);
            entity.Property(e => e.Malop)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Manh)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.MalopNavigation).WithMany(p => p.Hocsinhs)
                .HasForeignKey(d => d.Malop)
                .HasConstraintName("FK__Hocsinh__Malop__5070F446");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.Malop).HasName("PK__Lop__3313BBCD58BCC575");

            entity.ToTable("Lop");

            entity.Property(e => e.Malop)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Khoi).HasMaxLength(50);
            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Manh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Tenlop).HasMaxLength(20);

            entity.HasOne(d => d.MagvNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.Magv)
                .HasConstraintName("FK__Lop__Magv__4CA06362");

            entity.HasOne(d => d.ManhNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.Manh)
                .HasConstraintName("FK_Lopy_Namhoc");
        });

        modelBuilder.Entity<Monhoc>(entity =>
        {
            entity.HasKey(e => e.Mamon).HasName("PK__Monhoc__33DA29C25D92FFFE");

            entity.ToTable("Monhoc");

            entity.Property(e => e.Mamon)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Tenmon).HasMaxLength(50);
        });

        modelBuilder.Entity<Namhoc>(entity =>
        {
            entity.HasKey(e => e.Manh).HasName("PK__Namhoc__2724CB70F766727F");

            entity.ToTable("Namhoc");

            entity.Property(e => e.Manh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.Matk).HasName("PK__Taikhoan__7A217E16D538729C");

            entity.ToTable("Taikhoan");

            entity.Property(e => e.Matk)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("matk");
            entity.Property(e => e.Hoten).HasMaxLength(100);
            entity.Property(e => e.Matkhau)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Quyenhan).HasMaxLength(50);
            entity.Property(e => e.Tendangnhap)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
