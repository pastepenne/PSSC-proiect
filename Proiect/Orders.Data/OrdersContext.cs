using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;

namespace Orders.Data
{
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> Products { get; set; }
        public DbSet<OrderDto> Orders { get; set; }
        public DbSet<OrderItemDto> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product table (prepopulat)
            modelBuilder.Entity<ProductDto>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Code).HasMaxLength(20).IsRequired();
                entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.HasIndex(p => p.Code).IsUnique();
            });

            // Order table
            modelBuilder.Entity<OrderDto>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(o => o.ClientEmail).HasMaxLength(100).IsRequired();
                entity.Property(o => o.ShippingAddress).HasMaxLength(200).IsRequired();
                entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
                entity.HasIndex(o => o.OrderNumber).IsUnique();
            });

            // OrderItem table
            modelBuilder.Entity<OrderItemDto>(entity =>
            {
                entity.ToTable("OrderItem");
                entity.HasKey(i => i.OrderItemId);
                entity.Property(i => i.ProductCode).HasMaxLength(20).IsRequired();
                entity.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(i => i.TotalPrice).HasColumnType("decimal(18,2)");
                
                entity.HasOne(i => i.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(i => i.OrderId);
            });
        }
    }
}
