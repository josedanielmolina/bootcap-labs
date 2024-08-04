using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiAdmin.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Areasempresa> Areasempresas { get; set; }

    public virtual DbSet<Backlogsevent> Backlogsevents { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Areasempresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("areasempresa");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Backlogsevent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("backlogsevents");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CompleteAt).HasColumnType("datetime");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasColumnType("int(11) unsigned");
            entity.Property(e => e.Json).IsRequired();
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleados");

            entity.HasIndex(e => e.AreaEmpresaId, "FK_empleados_areasempresa");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AreaEmpresaId).HasColumnType("int(11)");
            entity.Property(e => e.Cargo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CodigoRH)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("CodigoRH");
            entity.Property(e => e.Correo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.AreaEmpresa).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.AreaEmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_empleados_areasempresa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
