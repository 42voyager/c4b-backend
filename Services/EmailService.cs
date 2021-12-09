using backend.Interfaces;
using backend.Models;
using MailKit.Net.Smtp;
using System.IO;
using MailKit;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
	public class EmailService<T> : IEmailService<T>
	{
		private readonly IConfiguration _configuration;
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void SendEmail(T newUser, int Id, Email email)
		{
			// We create the path of json file that will be attached to the email
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsondata-{Id}.json";
			this.PrepareCustomerJson(newUser, attachmentPath);

			var message = new MimeMessage();
			// To-do: Improve appsettings names
			message.From.Add(new MailboxAddress("Voyaguer", _configuration.GetSection("Email:MessageFrom").Value));
			// For now, We sent the email only to the Administrator. Later we plan to send a confirmation email to the customer
			message.To.Add(new MailboxAddress("Banco ABC Admin", _configuration.GetSection("Email:MessageTo").Value));

			message.Subject = email.Subject;
			var	builder = new BodyBuilder();
			builder.HtmlBody = email.Body;
			builder.Attachments.Add(attachmentPath);
			message.Body = builder.ToMessageBody();

			using (var client = new SmtpClient(new ProtocolLogger ("smtp.log")))
			{
				client.Connect(_configuration.GetSection("Email:SmtpHost").Value, 587, false);
				// Note: only needed if the SMTP server requires authentication
				client.Authenticate(_configuration.GetSection("Email:SmtpUser").Value, _configuration.GetSection("Email:SmtpPassword").Value);
				client.Send(message);
				System.Console.Write("Email sent to userid: " + Id + "!");
				client.Disconnect(true);
			}
			//Deleta o arquivo json após o envio do email
			if (File.Exists(attachmentPath))
				File.Delete(attachmentPath);
		}

		public void PrepareCustomerJson(T newUser, string file)
		{
			var responseData = newUser;
			var path = Directory.GetCurrentDirectory() + $"/JsonData";

			//cria a pasta JsonData caso não exista
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string jsonData = JsonConvert.SerializeObject(responseData, Formatting.None);
			System.IO.File.WriteAllText(file, jsonData);
		}
	}
}