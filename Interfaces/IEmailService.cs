using backend.Models;
using System.Collections.Generic;
using MimeKit;

namespace backend.Interfaces 
{
	public interface IEmailService<T>
	{
		void SendEmail(T newUser, int Id, Email email);

		//BodyBuilder GenerateBuilder(T newUser, string attachmentPath, int Id, string Company);

		void PrepareCustomerJson(T newUser, string file);

	}
}