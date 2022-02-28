
namespace backend.Models
{
	public class Email
	{
		public string Body { get; set; }

		public string Subject { get; set; }

		public string AttachmentPath { get; set; }

		public string RecipientEmail {get; set; }

		public string RecipientName {get; set;}
	}
}