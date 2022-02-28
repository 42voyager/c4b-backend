using System.Collections.Generic;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Services
{

	/// <summary>
	/// Class <c>CustomerService</c> implementa <c>ICustomerService</c> interface.
	/// </summary>
	public class CustomerService : ICustomerService
	{
		private readonly SellerContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly IEmailService<Customer> _emailService;

		/// <summary>
		/// Este construtor inicializa o <paramref name="context"/> do banco de dados,
		/// as <paramref name="configuration"/> do arquivo appSettings 
		/// e a instância do <paramref name="emailService"/>.
		/// </summary>
		/// <param name="context">Instância do banco de dados.</param>
		/// <param name="configuration">Instância do IConfiguration.</param>
		/// <param name="emailService">Instância do Serviço de email.</param>
		public CustomerService(
			SellerContext context,
			IConfiguration configuration,
			IEmailService<Customer> emailService
			)
		{
			_dbContext = context;
			_configuration = configuration;
			_emailService = emailService;
		}

		/// <inheritdoc />
		public async Task<List<Customer>> GetAllAsync()
		{
			return await _dbContext.Customers.ToListAsync();
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
		public async Task<Customer> GetAsync(int id)
		{
			return await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
		}

		/// <inheritdoc />
		public async Task<int> AddAsync(Customer newCustomer)
		{
			newCustomer.Status = Status.Credit;
			var result = await _dbContext.Customers.AddAsync(newCustomer);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
		}

		/// <inheritdoc />
		public async Task<bool> DeleteAsync(int id)
		{
			var Customer = await _dbContext.Customers.FindAsync(id);
			if (Customer == null)
				return false;
			_dbContext.Customers.Remove(Customer);
			await _dbContext.SaveChangesAsync();
			return (true);
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
		public async Task<bool> UpdateStatusAsync(int id, string status)
		{
			var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
			if (customer == null)
				return false;
			customer.Status = status;
			await _dbContext.SaveChangesAsync();
			return (true);
		}

		/// <inheritdoc />
		public async Task<Email> PrepareEmail(Customer customer)
		{
			DateTime localDate = DateTime.Now;
			string attachmentPath = Directory.GetCurrentDirectory() 
				+ $"/JsonData/jsonDataCustomer-{customer.Id}.json";
			var email = new Email();

			await _emailService.PrepareJsonAsync(customer, attachmentPath);
			email.AttachmentPath = attachmentPath;

			string[] templateBody = _configuration
				.GetSection("EmailTemplates:CustomerRequest:Body").Get<string[]>();
			email.Subject = string.Format(
				_configuration.GetSection("EmailTemplates:CustomerRequest:Subject").Value,
				customer.Company
			);
			email.Body = string.Format(
				String.Join(" ", templateBody),
				localDate.ToLongDateString(),
				customer.Company
			);
			email.RecipientEmail = _configuration.GetSection("Email:MessageTo:Email").Value;
			email.RecipientName = _configuration.GetSection("Email:MessageTo:Name").Value;
			return email;
		}
	}
}