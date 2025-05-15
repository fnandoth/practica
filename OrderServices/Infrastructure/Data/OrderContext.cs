
using Microsoft.EntityFrameworkCore;
using OrderServices.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderServices.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(ConfigureOrder);
            modelBuilder.Entity<OrderProduct>(ConfigureOrderProduct);
        }

        private void ConfigureOrder(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            builder.Property(o => o.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("UserName");
            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("TotalPrice");
            builder.HasMany(o => o.Products)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureOrderProduct(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProducts");
            builder.HasKey(op => op.Id);
            builder.Property(op => op.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            builder.Property(op => op.ProductId)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("ProductId");
            builder.Property(op => op.ProductName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("ProductName");
            builder.Property(op => op.Quantity)
                .IsRequired()
                .HasColumnName("Quantity");
            builder.Property(op => op.Price)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("Price");
        }
    }
}