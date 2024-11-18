using Microsoft.EntityFrameworkCore;

namespace DataAccessLib.Models;

public partial class DatabaseContext : DbContext
{
   public DatabaseContext()
   {
   }
   public DatabaseContext(DbContextOptions<DatabaseContext> options)
       : base(options)
   {
   }

   public virtual DbSet<Category> Categories { get; set; }
   public virtual DbSet<Product> Products { get; set; }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Category>(entity =>
      {
         entity.HasIndex(e => e.CategoryName, "CategoryName");

         entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
         entity.Property(e => e.CategoryName).HasMaxLength(15);
         entity.Property(e => e.Description).HasColumnType("ntext");
         entity.Property(e => e.Picture).HasColumnType("image");
      });

      modelBuilder.Entity<Product>(entity =>
      {
         entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

         entity.HasIndex(e => e.CategoryId, "CategoryID");

         entity.HasIndex(e => e.ProductName, "ProductName");

         entity.HasIndex(e => e.SupplierId, "SupplierID");

         entity.HasIndex(e => e.SupplierId, "SuppliersProducts");

         entity.Property(e => e.ProductId).HasColumnName("ProductID");
         entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
         entity.Property(e => e.ProductName).HasMaxLength(40);
         entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
         entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
         entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
         entity.Property(e => e.UnitPrice)
             .HasDefaultValue(0m)
             .HasColumnType("money");
         entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
         entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);

         entity.HasOne(d => d.Category).WithMany(p => p.Products)
             .HasForeignKey(d => d.CategoryId)
             .HasConstraintName("FK_Products_Categories")
             .OnDelete(DeleteBehavior.Cascade);
      });

      OnModelCreatingPartial(modelBuilder);
   }

   partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

