using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
	public class FeedbackContext : DbContext
	{
		public FeedbackContext(DbContextOptions<FeedbackContext> options) : base(options)
		{
		}

		public DbSet<Feedback> Feedback { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Feedback>().ToTable("Feedback");
		}
	}
}