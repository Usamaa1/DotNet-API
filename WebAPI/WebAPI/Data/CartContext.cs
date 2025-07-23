using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data;

public partial class CartContext : DbContext
{
    public CartContext()
    {
    }

    public CartContext(DbContextOptions<CartContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC070F74F9D5");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasColumnName("categoryName");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC079CE592B7");

            entity.ToTable("Product");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.ProdDesc).HasColumnName("prodDesc");
            entity.Property(e => e.ProdImage)
                .HasDefaultValue("No Image Found")
                .HasColumnName("prodImage");
            entity.Property(e => e.ProdName).HasColumnName("prodName");
            entity.Property(e => e.ProdPrice).HasColumnName("prodPrice");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__categor__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
