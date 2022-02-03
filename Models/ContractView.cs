using System.Text.Json.Serialization;

namespace backend.Models
{
	public class ContractView: Contract
	{
		[JsonIgnore]
		public string RecaptchaToken { get; set; }
	}
}