using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	public interface IFeedbackStarService
	{
		Task<List<FeedbackStar>> GetAllAsync();

		Task<FeedbackStar> GetAsync(int id);

		Task<int> AddAsync(FeedbackStar feedbackStar);

		Task<bool> DeleteAsync(int id);
	}
}