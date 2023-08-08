using System;
using System.Collections.Generic;
using Assesment_AmbilySajan.Models;
using Microsoft.EntityFrameworkCore;

namespace Assesment_AmbilySajan.Data;

public partial class TableDbCOntext : DbContext
{
    public TableDbCOntext()
    {
    }

    public TableDbCOntext(DbContextOptions<TableDbCOntext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aocolumn> AOColumn { get; set; }

    public virtual DbSet<Aotables> AOTable { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aocolumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_AOColumn_Id");

            entity.ToTable("AOColumn", tb => tb.HasTrigger("tr_AOColumn_Delete"));

            entity.HasIndex(e => e.Name, "ix_AOColumn_Name");

            entity.HasIndex(e => e.TableId, "ix_AOColumn_TableId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(2048);
            entity.Property(e => e.DataType).HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Distortion).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.Type).HasMaxLength(128);

            entity.HasOne(d => d.Table).WithMany(p => p.Aocolumns)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_AOColumn_AOTable");
        });

        modelBuilder.Entity<Aotables>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_AOTable_Id");

            entity.ToTable("AOTable");

            entity.HasIndex(e => e.Name, "ix_AOTable_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Boundary).HasDefaultValueSql("((0))");
            entity.Property(e => e.Cache).HasDefaultValueSql("((0))");
            entity.Property(e => e.Comment).HasMaxLength(2048);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.History).HasDefaultValueSql("((0))");
            entity.Property(e => e.Identifier).HasDefaultValueSql("((0))");
            entity.Property(e => e.Log).HasDefaultValueSql("((0))");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Notify).HasDefaultValueSql("((0))");
            entity.Property(e => e.Type).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
