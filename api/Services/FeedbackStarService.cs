using System.Collections.Generic;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Threading.Tasks;

namespace backend.Services
{
	/// <summary>
	/// Class <c>FeedbackStarService</c> implementa <c>IFeedbackStarService</c> interface.
	/// </summary>
	public class FeedbackStarService : IFeedbackStarService
	{
		private readonly SellerContext _dbContext;

		/// <summary>
		/// Este construtor inicializa o <paramref name="context"/> do banco de dados.
		/// </summary>
		/// <param name="context">Instância do banco de dados.</param>
		public FeedbackStarService(SellerContext context)
		{
			_dbContext = context;
		}

		/// <inheritdoc />
		public async Task<List<FeedbackStar>> GetAllAsync()
		{
			return await _dbContext.FeedbackStar.ToListAsync();
		}

		/// <inheritdoc />
		public async Task<FeedbackStar> GetAsync(int id)
		{
			return await _dbContext.FeedbackStar.FirstOrDefaultAsync(p => p.Id == id);
		}

		/// <inheritdoc />
		public async Task<int> AddAsync(FeedbackStar newFeedback)
		{
			var result = await _dbContext.FeedbackStar.AddAsync(newFeedback);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
		}

		/// <inheritdoc />
		public async Task<bool> DeleteAsync(int id)
		{
			var feedbackStar = await _dbContext.FeedbackStar.FindAsync(id);
			if (feedbackStar == null)
				return false;
			_dbContext.FeedbackStar.Remove(feedbackStar);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}