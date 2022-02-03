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
		public DbSet<Feedback> Feedback { get; set; }
		public DbSet<BankInfo> BankInfo { get; set; }
		public DbSet<Contract> Contracts { get; set; }
		public DbSet<FeedbackStar> FeedbackStar { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Customer>().ToTable("Customer");
			modelBuilder.Entity<Feedback>().ToTable("Feedback");
			modelBuilder.Entity<BankInfo>().ToTable("BankInfo");
			modelBuilder.Entity<Contract>().ToTable("Contract");
			modelBuilder.Entity<FeedbackStar>().ToTable("FeedbackStar");
		}
	}
}