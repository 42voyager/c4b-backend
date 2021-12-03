using backend.Models;
using System.Collections.Generic;

namespace backend.Interfaces
{
	public interface IFeedbackService
	{
		List<Feedback> GetAll();

		Feedback Get(int id);

		int Add(Feedback newFeedback);

		bool Delete(int id);
	}
}