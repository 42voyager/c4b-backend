using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
	public class SellerContext : DbContext
	{
		public SellerContext(DbContextOptions<SellerContext> options) : base(options)
		{
		}

		public DbSet<Customer> Customers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Customer>().ToTable("Customer");
		}
	}
}