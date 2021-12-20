using System.Collections.Generic;
using System.Linq;
using backend.Interfaces;
using backend.Models;
using backend.Data;

namespace backend.Services
{
	public class FeedbackService : IFeedbackService
	{
		private readonly SellerContext _dbContext;

		public FeedbackService(SellerContext context)
		{
			_dbContext = context;
		}

		public List<Feedback> GetAll()
		{
			return _dbContext.Feedback.ToList();
		}

		public Feedback Get(int id)
		{
			return _dbContext.Feedback.FirstOrDefault(p => p.Id == id);
		}

		public int Add(Feedback newFeedback)
		{
			var result = _dbContext.Feedback.Add(newFeedback);
			_dbContext.SaveChanges();
			return result.Entity.Id;
		}

		public bool Delete(int id)
		{
			var feedback = _dbContext.Feedback.Find(id);
			if (feedback == null)
				return false;
			_dbContext.Feedback.Remove(feedback);
			_dbContext.SaveChanges();
			return true;
		}
	}
}