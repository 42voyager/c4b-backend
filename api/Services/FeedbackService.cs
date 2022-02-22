using System.Collections.Generic;
using System.Linq;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class FeedbackService : IFeedbackService
	{
		private readonly SellerContext _dbContext;

		public FeedbackService(SellerContext context)
		{
			_dbContext = context;
		}

		public async Task<List<Feedback>> GetAllAsync()
		{
			return await _dbContext.Feedback.ToListAsync();
		}

		public async Task<Feedback> GetAsync(int id)
		{
			return await _dbContext.Feedback.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<int> AddAsync(Feedback newFeedback)
		{
			var result = await _dbContext.Feedback.AddAsync(newFeedback);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
		}
		
		public async Task<bool> DeleteAsync(int id)
		{
			var feedback = await _dbContext.Feedback.FindAsync(id);
			if (feedback == null)
				return false;
			_dbContext.Feedback.Remove(feedback);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}