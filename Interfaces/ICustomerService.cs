using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces 
{
	public interface ICustomerService
	{
		Task<List<Customer>> GetAllAsync();

		Task<Customer> GetAsync(int id);

		Task<int> AddAsync(Customer newCustomer);

		Task<bool> DeleteAsync(int id);

		Task<bool> UpdateAsync(Customer updateCustomer);

		Task<bool> UpdateStatusAsync(int id, string status);
	}
}