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
		SellerContext DbContext;
		EmailService EmailService;
		public CustomerService(SellerContext context)
		{
			DbContext = context;
			EmailService = new EmailService();
		}

		public List<Customer> GetAll()
		{
			return DbContext.Customers.ToList();
		}

		public Customer Get(int id)
		{
			return DbContext.Customers.FirstOrDefault(p => p.Id == id);
		}

		public bool Add(Customer newCustomer)
		{
			
			DbContext.Customers.Add(newCustomer);
			DbContext.SaveChanges();
			EmailService.SendEmail(newCustomer);
			return (true);
		}

		public bool Delete(int id)
		{
			var Customer = DbContext.Customers.Find(id);
			if (Customer == null)
				return false;
			DbContext.Customers.Remove(Customer);
			DbContext.SaveChanges();
			return (true);
		}

		public bool Update(Customer updateCustomer)
		{
			var customer = DbContext.Customers.FirstOrDefault(p => p.Id == updateCustomer.Id);
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
			DbContext.SaveChanges();
			return (true);
		}
	}
}