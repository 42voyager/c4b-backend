using System.Collections.Generic;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{

	/// <summary>
	/// Class <c>FeedbackService</c> implementa <c>IFeedbackService</c> interface.
	/// </summary>
	public class FeedbackService : IFeedbackService
	{
		private readonly SellerContext _dbContext;

		/// <summary>
		/// Este construtor inicializa o <paramref name="context"/> do banco de dados.
		/// </summary>
		/// <param name="context">Instância do banco de dados</param>
		public FeedbackService(SellerContext context)
		{
			_dbContext = context;
		}

		/// <inheritdoc />

		public async Task<List<Feedback>> GetAllAsync()
		{
			return await _dbContext.Feedback.ToListAsync();
		}

		/// <inheritdoc />
		public async Task<Feedback> GetAsync(int id)
		{
			return await _dbContext.Feedback.FirstOrDefaultAsync(p => p.Id == id);
		}

		/// <inheritdoc />
		public async Task<int> AddAsync(Feedback newFeedback)
		{
			var result = await _dbContext.Feedback.AddAsync(newFeedback);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
		}

		/// <inheritdoc />
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