using backend.Models;
using System.Collections.Generic;


namespace backend.Interfaces 
{
	public interface IEmailService
	{
		void SendEmail(Customer newCustomer);
	}
}