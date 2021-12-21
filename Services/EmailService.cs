using backend.Interfaces;
using backend.Models;
using MailKit.Net.Smtp;
using System.IO;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Services
{
	public class EmailService<T> : IEmailService<T> where T : class
	{
		private readonly IConfiguration _configuration;
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(Email email)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Voyaguer", _configuration.GetSection("Email:MessageFrom").Value));
			// For now, We sent the email only to the Administrator. Later we plan to send a confirmation email to the customer
			message.To.Add(new MailboxAddress("Banco ABC Admin", _configuration.GetSection("Email:MessageTo").Value));
			message.Subject = email.Subject;
			var	builder = new BodyBuilder();
			builder.HtmlBody = email.Body;
			await builder.Attachments.AddAsync(email.AttachmentPath);
			message.Body = builder.ToMessageBody();

			using (var client = new SmtpClient(new ProtocolLogger ("smtp.log")))
			{
				await client.ConnectAsync(_configuration.GetSection("Email:SmtpHost").Value, 587, false);
				// Note: only needed if the SMTP server requires authentication
				await client.AuthenticateAsync(_configuration.GetSection("Email:SmtpUser").Value, _configuration.GetSection("Email:SmtpPassword").Value);
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
			//Deleta o arquivo json após o envio do email
			if (File.Exists(email.AttachmentPath))
				File.Delete(email.AttachmentPath);
		}

		public async Task PrepareCustomerJsonAsync(T newUser, string file) 
		{
			var responseData = newUser;
			var path = Directory.GetCurrentDirectory() + $"/JsonData";

			//cria a pasta JsonData caso não exista
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			 JsonSerializerOptions options = new()
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
			};
			string jsonData = System.Text.Json.JsonSerializer.Serialize(responseData, options);
			await System.IO.File.WriteAllTextAsync(file, jsonData);
		}
	}
}