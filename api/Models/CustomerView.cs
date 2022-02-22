
using System.Text.Json.Serialization;

namespace backend.Models
{
	public class CustomerView: Customer
	{
		[JsonIgnore]
		public string RecaptchaToken { get; set; }
	}
}
