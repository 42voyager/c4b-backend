using System.Text.Json.Serialization;

namespace backend.Models
{
	public class FeedbackStar: Feedback
	{
		public int RateStar { set; get; }

	}
}