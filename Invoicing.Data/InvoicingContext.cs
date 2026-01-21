using Microsoft.EntityFrameworkCore;
using Invoicing.Data.Models;

namespace Invoicing.Data
{
    public class InvoicingContext : DbContext
    {
        public InvoicingContext(DbContextOptions<InvoicingContext> options) : base(options)
        {
        }

        public DbSet<InvoiceDto> Invoices { get; set; }
        public DbSet<InvoiceItemDto> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Invoice table
            modelBuilder.Entity<InvoiceDto>(entity =>
            {
                entity.ToTable("Invoice");
                entity.HasKey(i => i.InvoiceId);
                entity.Property(i => i.InvoiceNumber).HasMaxLength(50).IsRequired();
                entity.Property(i => i.OrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(i => i.ClientEmail).HasMaxLength(100).IsRequired();
                entity.Property(i => i.ShippingAddress).HasMaxLength(200).IsRequired();
                entity.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
                entity.HasIndex(i => i.InvoiceNumber).IsUnique();
            });

            // InvoiceItem table
            modelBuilder.Entity<InvoiceItemDto>(entity =>
            {
                entity.ToTable("InvoiceItem");
                entity.HasKey(i => i.InvoiceItemId);
                entity.Property(i => i.ProductCode).HasMaxLength(20).IsRequired();
                entity.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(i => i.TotalPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(i => i.Invoice)
                      .WithMany(inv => inv.Items)
                      .HasForeignKey(i => i.InvoiceId);
            });
        }
    }
}
