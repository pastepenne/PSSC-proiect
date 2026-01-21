using Microsoft.EntityFrameworkCore;
using Shipping.Data.Models;

namespace Shipping.Data
{
    public class ShippingContext : DbContext
    {
        public ShippingContext(DbContextOptions<ShippingContext> options) : base(options)
        {
        }

        public DbSet<ShipmentDto> Shipments { get; set; }
        public DbSet<ShipmentItemDto> ShipmentItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // tabel Shipment 
            modelBuilder.Entity<ShipmentDto>(entity =>
            {
                entity.ToTable("Shipment");
                entity.HasKey(s => s.ShipmentId);
                entity.Property(s => s.TrackingNumber).HasMaxLength(50).IsRequired();
                entity.Property(s => s.OrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(s => s.InvoiceNumber).HasMaxLength(50).IsRequired();
                entity.Property(s => s.CourierName).HasMaxLength(50).IsRequired();
                entity.Property(s => s.ClientEmail).HasMaxLength(100).IsRequired();
                entity.Property(s => s.ShippingAddress).HasMaxLength(200).IsRequired();
                entity.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
                entity.HasIndex(s => s.TrackingNumber).IsUnique();
            });

            // tabel ShipmentItem
            modelBuilder.Entity<ShipmentItemDto>(entity =>
            {
                entity.ToTable("ShipmentItem");
                entity.HasKey(i => i.ShipmentItemId);
                entity.Property(i => i.ProductCode).HasMaxLength(20).IsRequired();
                entity.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(i => i.TotalPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(i => i.Shipment)
                      .WithMany(s => s.Items)
                      .HasForeignKey(i => i.ShipmentId);
            });
        }
    }
}
