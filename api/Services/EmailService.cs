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

	/// <summary>
	/// Class <c>EmailService</c> implementa <c>IEmailService</c> interface.
	/// Esta classe é genérica, assim você pode criar com qualquer tipo.
	/// </summary>
	/// <typeparam name="T">O tipo armazenado pela classe.</typeparam>
	public class EmailService<T> : IEmailService<T> where T : class
	{
		private readonly IConfiguration _configuration;

		/// <summary>
		/// Este construtor inicializa as <paramref name="configuration"/> do arquivo appSettings.
		/// </summary>
		/// <param name="configuration">Instância do IConfiguration.</param>
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <inheritdoc />

		public async Task SendEmailAsync(Email email)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Voyaguer", _configuration.GetSection("Email:MessageFrom").Value));
			// For now, We sent the email only to the Administrator. Later we plan to send a confirmation email to the customer
			message.To.Add(new MailboxAddress("Banco ABC Admin", email.Recipient));
			message.Subject = email.Subject;
			var	builder = new BodyBuilder();
			builder.HtmlBody = email.Body;
			if (email.AttachmentPath != null)
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

		/// <inheritdoc />
		public async Task PrepareJsonAsync(T entity, string file)
		{
			var responseData = entity;
			var path = Directory.GetCurrentDirectory() + $"/JsonData";

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