using Microsoft.EntityFrameworkCore;
using Ecommerce_API.Models;
using Ecommerce_API.Entities;

namespace Ecommerce_API.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Email for Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Product and Category Relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductImage Relationship
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId);

            // ✅ Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Men", CreatedOn = DateTime.Now },
                new Category { Id = 2, Name = "Women", CreatedOn = DateTime.Now },
                new Category { Id = 3, Name = "Kids", CreatedOn = DateTime.Now }
            );

            // ✅ Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Denim Jacket", Description = "Classic Blue Denim Jacket", Price = 2499, CategoryId = 1, CurrentStock = 50, CreatedOn = DateTime.Now },
                new Product { Id = 2, Name = "Red Hoodie", Description = "Unisex Red Hoodie", Price = 1799, CategoryId = 2, CurrentStock = 80, CreatedOn = DateTime.Now },
                new Product { Id = 3, Name = "Kids Cartoon Tee", Description = "Soft cotton T-shirt for kids", Price = 599, CategoryId = 3, CurrentStock = 100, CreatedOn = DateTime.Now }
            );

            // ✅ Seed Product Images
            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://via.placeholder.com/300", PublicId = "demo1", IsMain = true, CreatedOn = DateTime.Now },
                new ProductImage { Id = 2, ProductId = 2, ImageUrl = "https://via.placeholder.com/300", PublicId = "demo2", IsMain = true, CreatedOn = DateTime.Now },
                new ProductImage { Id = 3, ProductId = 3, ImageUrl = "https://via.placeholder.com/300", PublicId = "demo3", IsMain = true, CreatedOn = DateTime.Now }
            );
        }

    }
}


