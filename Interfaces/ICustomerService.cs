using backend.Models;
using System.Collections.Generic;


namespace backend.Interfaces 
{
	public interface ICustomerService
	{
		List<Customer> GetAll();

		Customer Get(int id);

		int Add(Customer newCustomer);

		bool Delete(int id);

		bool Update(Customer updateCustomer);
	}
}