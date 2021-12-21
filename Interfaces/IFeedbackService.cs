using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	public interface IFeedbackService
	{
		Task<List<Feedback>> GetAllAsync();

		Task<Feedback> GetAsync(int id);

		Task<int> AddAsync(Feedback newFeedback);

		Task<bool> DeleteAsync(int id);
	}
}