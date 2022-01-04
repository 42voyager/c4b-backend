using System.Collections.Generic;
using backend.Services;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly SellerContext _dbContext;
		public CustomerService(SellerContext context)
		{
			_dbContext = context;
		}

		public async Task<List<Customer>> GetAllAsync()
		{
			return await _dbContext.Customers.ToListAsync();
		}

		public async Task<Customer> GetAsync(int id)
		{
			return await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<int> AddAsync(Customer newCustomer)
		{
			var result = await _dbContext.Customers.AddAsync(newCustomer);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var Customer = await _dbContext.Customers.FindAsync(id);
			if (Customer == null)
				return false;
			_dbContext.Customers.Remove(Customer);
			await _dbContext.SaveChangesAsync();
			return (true);
		}

		public async Task<bool> UpdateAsync(Customer updateCustomer)
		{
			var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == updateCustomer.Id);
			if (customer == null)
				return false;
			customer.Limit = updateCustomer.Limit;
			customer.Installment = updateCustomer.Installment;
			customer.Reason = updateCustomer.Reason;
			customer.Name = updateCustomer.Name;
			customer.Email = updateCustomer.Email;
			customer.Cellphone = updateCustomer.Cellphone;
			customer.Cnpj = updateCustomer.Cnpj;
			customer.Company = updateCustomer.Company;
			customer.Optin = updateCustomer.Optin;
			customer.IPAddress = updateCustomer.IPAddress;
			customer.OperatingSystem = updateCustomer.OperatingSystem;
			customer.Timestamp = updateCustomer.Timestamp;
			await _dbContext.SaveChangesAsync();
			return (true);
		}
	}
}