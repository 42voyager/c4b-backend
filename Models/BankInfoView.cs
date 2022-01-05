using System.Text.Json.Serialization;

namespace backend.Models
{
	public class BankInfoView: BankInfo
	{
		[JsonIgnore]
		public string RecaptchaToken { get; set; }
	}
}