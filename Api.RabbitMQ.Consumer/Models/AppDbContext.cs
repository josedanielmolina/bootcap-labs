using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.RabbitMQ.Consumer.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Areasempresa> Areasempresas { get; set; }

    public virtual DbSet<Backlogsevent> Backlogsevents { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Transferencia> Transferencias { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Areasempresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("areasempresa");

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Backlogsevent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("backlogsevents");

            entity.Property(e => e.CompleteAt).HasColumnType("datetime");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Json).IsRequired();
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleados");

            entity.HasIndex(e => e.AreaEmpresaId, "FK_empleados_areasempresa");

            entity.Property(e => e.Cargo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CodigoRh)
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

        modelBuilder.Entity<Transferencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transferencias");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasPrecision(20, 6);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "correo").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoValidacion).HasMaxLength(255);
            entity.Property(e => e.Contrasena)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaExpiracionCodigo).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
