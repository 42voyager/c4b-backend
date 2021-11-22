using backend.Interfaces;
using backend.Models;
using MailKit.Net.Smtp;
using System.IO;
using MailKit;
using MimeKit;
using Newtonsoft.Json;

namespace backend.Services
{
	public class EmailService : IEmailService
	{
		public void SendEmail(Customer newCustomer)
		{
			// We create the json file that will be attached to the email
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsondata-{newCustomer.Id}.json";
			this.PrepareCustomerJson(newCustomer, attachmentPath);

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Voyaguer", "labs.voyaguer@gmail.com"));
			// For now, We sent the email only to the Administrator. Later we plan to send a confirmation email to the customer
			message.To.Add(new MailboxAddress("Banco ABC Admin", "buccky.live8@gmail.com"));

			message.Subject = "Nova solicitacao de credito: Usuario " + newCustomer.Name;

			var builder = new BodyBuilder();
			builder.TextBody = @$"Hey Marcus, enviamos as informações do usuário: {newCustomer.Name}, está tudo no anexo";
			builder.Attachments.Add(attachmentPath);
			message.Body = builder.ToMessageBody();

			using (var client = new SmtpClient(new ProtocolLogger ("smtp.log")))
			{
				client.Connect("smtp.gmail.com", 587, false);
				// Note: only needed if the SMTP server requires authentication
				client.Authenticate("labs.voyaguer", "voyaguer123");
				client.Send(message);
				System.Console.Write("Email sent to userid: " + newCustomer.Id + "!");
				client.Disconnect(true);
			}
		}

		private void PrepareCustomerJson(Customer newCustomer, string path)
		{
			var responseData = newCustomer;
			string jsonData = JsonConvert.SerializeObject(responseData, Formatting.None);
			System.IO.File.WriteAllText(path, jsonData);
		}
	}
}