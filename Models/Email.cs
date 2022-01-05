
namespace backend.Models
{
	public class Email
	{
		public string Body { get; set; }

		public string Subject { get; set; }

		public string AttachmentPath { get; set; }

		public string Recipient {get; set; }
	}
}