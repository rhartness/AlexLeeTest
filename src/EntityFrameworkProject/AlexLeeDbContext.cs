using System;
using System.Collections.Generic;
using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkProject;

public partial class AlexLeeDbContext : DbContext
{
    public AlexLeeDbContext()
    {
    }

    public AlexLeeDbContext(DbContextOptions<AlexLeeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PurchaseDetailItem> PurchaseDetailItems { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.\\SqlExpress;Database=AlexLee;Integrated Security=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseDetailItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PurchaseDetailItem");

            entity.Property(e => e.ItemDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedByUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.PurchaseDetailItemAutoId).ValueGeneratedOnAdd();
            entity.Property(e => e.PurchaseOrderNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
