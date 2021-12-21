using backend.Models;
using System.Collections.Generic;
using MimeKit;
using System.Threading.Tasks;

namespace backend.Interfaces 
{
	public interface IEmailService<T>
	{
		Task SendEmailAsync(Email email);

		//BodyBuilder GenerateBuilder(T newUser, string attachmentPath, int Id, string Company);

		Task PrepareCustomerJsonAsync(T newUser, string file);

	}
}