using System.Text.Json.Serialization;

namespace backend.Models
{
	public class FeedbackView: Feedback
	{
		[JsonIgnore]
		public string RecaptchaToken { get; set; }

	}
}