using System;
using Microsoft.EntityFrameworkCore;
using CarInsuranceQuoteSystem.Models;

namespace CarInsuranceQuoteSystem.Data
{
	public class AppDbContext : DbContext
	{

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Quote> Quotes { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Quotes)
                .WithOne(q => q.Customer)
                .HasForeignKey(q => q.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Customer)
                .WithMany(c => c.Quotes)
                .HasForeignKey(q => q.CustomerId);
        }
    }
}

