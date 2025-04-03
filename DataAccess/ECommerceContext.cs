using Asm03Solution.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ECommerceContext
    : DbContext
    {
 

		public ECommerceContext(DbContextOptions<ECommerceContext> options)
			: base(options)
		{
		}

		public DbSet<Member> Members { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite key for OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            // Configure relationships
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Member)
                .WithMany(m => m.Orders)
                .HasForeignKey(o => o.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure non-nullable strings
            modelBuilder.Entity<Member>().Property(m => m.Email).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.CompanyName).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.City).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.Country).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.Password).IsRequired();

            modelBuilder.Entity<Product>().Property(p => p.ProductName).IsRequired();

            modelBuilder.Entity<Category>().Property(c => c.CategoryName).IsRequired();
        }
    }
}
