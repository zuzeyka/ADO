using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lesson1.Scaffolded;

public partial class DCWpfLesson1Lesson1Ado201MdfContext : DbContext
{
    public DCWpfLesson1Lesson1Ado201MdfContext()
    {
    }

    public DCWpfLesson1Lesson1Ado201MdfContext(DbContextOptions<DCWpfLesson1Lesson1Ado201MdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\C#\\WPF\\Lesson1\\Lesson1\\ADO-201.mdf;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07217C3190");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Managers__3214EC07F3AA481B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IdChief).HasColumnName("Id_chief");
            entity.Property(e => e.IdMainDep).HasColumnName("Id_main_dep");
            entity.Property(e => e.IdSecDep).HasColumnName("Id_sec_dep");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Secname).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.IdMainDepNavigation).WithMany(p => p.ManagerIdMainDepNavigations)
                .HasForeignKey(d => d.IdMainDep)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Id_mai__6FE99F9F");

            entity.HasOne(d => d.IdSecDepNavigation).WithMany(p => p.ManagerIdSecDepNavigations)
                .HasForeignKey(d => d.IdSecDep)
                .HasConstraintName("FK__Managers__Id_sec__70DDC3D8");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC079D752B32");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sales__3214EC0764512900");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");
            entity.Property(e => e.SellTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdManager)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sales__IdManager__05D8E0BE");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sales__IdProduct__04E4BC85");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
