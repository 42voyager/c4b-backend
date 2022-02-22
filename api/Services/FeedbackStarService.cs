using System.Collections.Generic;
using System.Linq;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class FeedbackStarService : IFeedbackStarService
	{
		private readonly SellerContext _dbContext;

		public FeedbackStarService(SellerContext context)
		{
			_dbContext = context;
		}

		/// <summary>
		/// Pega do banco de dados todos os feedback com estrela.
		/// Em modo Async.
		/// </summary>
		/// <returns> Retorna uma lista com todos os feedback de
		/// estrela salvo no banco de dados </returns>
		public async Task<List<FeedbackStar>> GetAllAsync()
		{
			return await _dbContext.FeedbackStar.ToListAsync();
		}

		/// <summary>
		/// Pega do banco de dados o feedback com estrela do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do feedback desejado. </param>
		/// <returns> Retorna o feedback desejado. </returns>
		public async Task<FeedbackStar> GetAsync(int id)
		{
			return await _dbContext.FeedbackStar.FirstOrDefaultAsync(p => p.Id == id);
		}

		/// <summary>
		/// Salva no banco de dados o novo feedback recebido.
		/// Em modo Async.
		/// </summary>
		/// <param name="newFeedback"> Novo feedback com estrela. </param>
		/// <returns> retornna o id do feedback no banco de dados. </returns>
		public async Task<int> AddAsync(FeedbackStar newFeedback)
		{
			var result = await _dbContext.FeedbackStar.AddAsync(newFeedback);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
		}
		
		/// <summary>
		/// Delete do banco de dados o feedback com estrela do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do feedback de estrela a ser deletado.</param>
		/// <returns> Retorna true se conseguir deletar, e false se não existir.</returns>
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