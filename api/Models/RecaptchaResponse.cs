namespace backend.Models
{
	public class RecaptchaResponse
	{
		public bool success { get; set; }
		public System.DateTime challenge_ts { get; set; }
		public string hostname { get; set; }
		public double score { get; set; }
		public string action { get; set; }
	}
}