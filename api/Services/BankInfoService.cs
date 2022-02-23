using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace backend.Services
{
	public class BankInfoService : IBankInfoService
	{
		private readonly SellerContext _dbContext;

		private readonly IConfiguration _configuration;

		public BankInfoService(SellerContext context, IConfiguration configuration)
		{
			_dbContext = context;
			_configuration = configuration;
		}
		public async Task<int> AddAsync(BankInfo bankInfo)
		{
			var result = await _dbContext.BankInfo.AddAsync(bankInfo);
			await _dbContext.SaveChangesAsync();
			return result.Entity.CustomerID;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var result = await _dbContext.BankInfo.FindAsync(id);
			if (result == null)
				return false;
			_dbContext.Remove(result);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<List<BankInfo>> GetAllAsync()
		{
			return await _dbContext.BankInfo.ToListAsync();
		}

		public async Task<BankInfo> GetAsync(int id)
		{
			return await _dbContext.BankInfo.FirstOrDefaultAsync(p => p.CustomerID == id);
		}

		public Email PrepareEmail(Customer customer, string hash)
		{
			var email = new Email();
			// The email template coming from the appsettings requires a hash value
			string[] templateBody = _configuration.GetSection("EmailTemplates:BankInfo:Body").Get<string[]>();

			email.Subject = _configuration.GetSection("EmailTemplates:BankInfo:Subject").Value;
			email.Body = string.Format(
				String.Join(" ", templateBody),
				hash
			);
			email.Recipient = customer.Email;
			return email;
		}
	}
}