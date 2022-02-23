using System.Collections.Generic;
using backend.Services;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly SellerContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly IEmailService<Customer> _emailService;
		public CustomerService(SellerContext context, IConfiguration configuration, IEmailService<Customer> emailService)
		{
			_dbContext = context;
			_configuration = configuration;
			_emailService = emailService;
		}

		public async Task<List<Customer>> GetAllAsync()
		{
			return await _dbContext.Customers.ToListAsync();
		}
		public async Task<List<CustomerReport>> GetAllReportAsync()
		{
			var customers = _dbContext.Customers
				.Select(p => new CustomerReport{ 
					Name = p.Name,
					Email = p.Email, 
					Cellphone = p.Cellphone,
					Cnpj = p.Cnpj,
					Company = p.Company,
					Limit = p.Limit,
					Installment = p.Installment,
					Reason = p.Reason,
					Status = p.Status
				});
			return await customers.ToListAsync();
		}

		public async Task<Customer> GetAsync(int id)
		{
			return await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<int> AddAsync(Customer newCustomer)
		{
			newCustomer.Status = Status.Credit;
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
		public async Task<bool> UpdateStatusAsync(int id, string status)
		{
			var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
			if (customer == null)
				return false;
			customer.Status = status;
			await _dbContext.SaveChangesAsync();
			return (true);
		}
		public async Task<Email> PrepareEmail(Customer customer)
		{
			DateTime localDate = DateTime.Now;
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsonDataCustomer-{customer.Id}.json";
			var email = new Email();

			await _emailService.PrepareJsonAsync(customer, attachmentPath);
			email.AttachmentPath = attachmentPath;

			string[] templateBody = _configuration.GetSection("EmailTemplates:CustomerRequest:Body").Get<string[]>();
			email.Subject = string.Format(
				_configuration.GetSection("EmailTemplates:CustomerRequest:Subject").Value,
				customer.Company
			);
			email.Body = string.Format(
				String.Join(" ", templateBody),
				localDate.ToLongDateString(),
				customer.Company
			);
			email.Recipient = _configuration.GetSection("Email:MessageTo").Value;
			return email;
		}
	}
}