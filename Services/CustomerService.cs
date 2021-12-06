using System.Collections.Generic;
using backend.Services;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Linq;

namespace backend.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly SellerContext _dbContext;
		public CustomerService(SellerContext context)
		{
			_dbContext = context;
		}

		public List<Customer> GetAll()
		{
			return _dbContext.Customers.ToList();
		}

		public Customer Get(int id)
		{
			return _dbContext.Customers.FirstOrDefault(p => p.Id == id);
		}

		public int Add(Customer newCustomer)
		{
			var result = _dbContext.Customers.Add(newCustomer);
			_dbContext.SaveChanges();
			return result.Entity.Id;
		}

		public bool Delete(int id)
		{
			var Customer = _dbContext.Customers.Find(id);
			if (Customer == null)
				return false;
			_dbContext.Customers.Remove(Customer);
			_dbContext.SaveChanges();
			return (true);
		}

		public bool Update(Customer updateCustomer)
		{
			var customer = _dbContext.Customers.FirstOrDefault(p => p.Id == updateCustomer.Id);
			if (customer == null)
				return false;
			customer.Limit = updateCustomer.Limit;
			customer.Installment = updateCustomer.Installment;
			customer.Name = updateCustomer.Name;
			customer.Email = updateCustomer.Email;
			customer.Cellphone = updateCustomer.Cellphone;
			customer.Cnpj = updateCustomer.Cnpj;
			customer.Company = updateCustomer.Company;
			customer.Optin = updateCustomer.Optin;
			customer.IP = updateCustomer.IP;
			customer.OS = updateCustomer.OS;
			customer.timestamp = updateCustomer.timestamp;
			_dbContext.SaveChanges();
			return (true);
		}
	}
}