using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoSoftware.Back.BE.Models;

public partial class ProyectoSoftwareDbContext : DbContext
{
    public ProyectoSoftwareDbContext()
    {
    }

    public ProyectoSoftwareDbContext(DbContextOptions<ProyectoSoftwareDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sex> Sexes { get; set; }

    public virtual DbSet<User> Users { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonaId).HasName("PK__Person__7C34D303E35957BB");

            entity.ToTable("Person");

            entity.HasIndex(e => e.Identification, "UQ__Person__724F06FD941F55F0").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.DateCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateModificate).HasColumnType("datetime");
            entity.Property(e => e.Identification).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.MaritalStatus).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.Occupation).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UserCreate).HasColumnName("User_Create");
            entity.Property(e => e.UserModificate).HasColumnName("User_Modificate");

            entity.HasOne(d => d.Sex).WithMany(p => p.People)
                .HasForeignKey(d => d.SexId)
                .HasConstraintName("FK_Person_Gender");

            entity.HasOne(d => d.User).WithMany(p => p.People)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Person_User");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__F92302F10DB89DC1");

            entity.ToTable("Rol");

            entity.HasIndex(e => e.DescriptionRol, "UQ__Rol__8DE351F26202A8D9").IsUnique();

            entity.Property(e => e.DateCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateModificate).HasColumnType("datetime");
            entity.Property(e => e.DescriptionRol).HasMaxLength(50);
            entity.Property(e => e.UserCreate).HasColumnName("User_Create");
            entity.Property(e => e.UserModificate).HasColumnName("User_Modificate");
        });

        modelBuilder.Entity<Sex>(entity =>
        {
            entity.HasKey(e => e.SexId).HasName("PK__Sex__75622D96D9C3C61D");

            entity.ToTable("Sex");

            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CC0D00FD2");

            entity.ToTable("User");

            entity.HasIndex(e => e.NameUser, "UQ__User__13F18517AE6DDB62").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534F454B307").IsUnique();

            entity.Property(e => e.DateCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateLastAttempts).HasColumnType("datetime");
            entity.Property(e => e.DateModificate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.NameUser).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RecoveredToken)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.UserCreate).HasColumnName("User_Create");
            entity.Property(e => e.UserModificate).HasColumnName("User_Modificate");

            entity.HasOne(d => d.Rol).WithMany(p => p.Users)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK_User_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
