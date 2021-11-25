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
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void SendEmail(Customer newCustomer)
		{
			// We create the path of json file that will be attached to the email
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsondata-{newCustomer.Id}.json";
			this.PrepareCustomerJson(newCustomer, attachmentPath);

			var message = new MimeMessage();
			// To-do: Improve appsettings names
			message.From.Add(new MailboxAddress("Voyaguer", _configuration.GetSection("Email:NomeEmail").Value));
			// For now, We sent the email only to the Administrator. Later we plan to send a confirmation email to the customer
			message.To.Add(new MailboxAddress("Banco ABC Admin", "buccky.live8@gmail.com"));

			message.Subject = "Nova solicitacao de credito: Usuario " + newCustomer.Name;

			var builder = this.GenerateBuilder(newCustomer, attachmentPath);
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
			//Deleta o arquivo json após o envio do email
			if (File.Exists(attachmentPath))
				File.Delete(attachmentPath);
		}

		private BodyBuilder GenerateBuilder(Customer newCustomer, String attachmentPath)
		{
			DateTime localDate = DateTime.Now;

			var builder = new BodyBuilder();
			builder.HtmlBody = string.Format(
				@$"<div style='background-color: #b29475; width: 100%; padding: 50px 30px; text-align: center;'>
				<h1 style='font-size= 14px; '>Pedido - {localDate.ToString()} <br>id - {newCustomer.Id}</h1> 
				<p>Nova solicitação de crédito, feita pelo usuário {newCustomer.Name}</p>
				<p>Todas as informações disponiveis estão guardadas no json anexado!</p>
				<p></p><br>
				<p style='margin: 40px; padding: 20px; background: white; color: #b29475;'>Equipe Voyager.</p>
				</div>");
			builder.Attachments.Add(attachmentPath);
			return (builder);
		}

		private void PrepareCustomerJson(Customer newCustomer, string file)
		{
			var responseData = newCustomer;
			var path = Directory.GetCurrentDirectory() + $"/JsonData";

			//cria a pasta JsonData caso não exista
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string jsonData = JsonConvert.SerializeObject(responseData, Formatting.None);
			System.IO.File.WriteAllText(file, jsonData);
		}
	}
}