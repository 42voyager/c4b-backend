using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class BankInfoService : IBankInfoService
	{
		private readonly SellerContext _dbContext;

		public BankInfoService(SellerContext context)
		{
			_dbContext = context;
		}
		public async Task<int> AddAsync(BankInfo bankInfo)
		{
			var result = await _dbContext.BankInfo.AddAsync(bankInfo);
			await _dbContext.SaveChangesAsync();
			return result.Entity.Id;
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
			return await _dbContext.BankInfo.FirstOrDefaultAsync(p => p.Id == id);
		}

		public Email PrepareEmail(Customer customer, string hash)
		{
			var email = new Email();

			email.Subject = $"Dados Bancários Para Contratar o Empréstimo";
			email.Body = string.Format(
					@$"<div style='max-width: 100%; width: calc(100% - 60px); padding: 30px 30px; text-align: center;'>
					<h1 style='font-size= 14px; '>Dados Bancários <br></h1> 
					<p>No link, você deve preencher seus dados bancários para fazer o empréstimo </p>
					<a href='www.c4b.fun/bankInfoForm/{hash}'> www.c4b.fun/bankInfoForm/{hash} </a>
					<p></p><br>
					<hr style='border: 2px solid #b29475;'>
  					<p style='padding: 10px; color: #b29475;'>Equipe C4B.</p>
					</div>");
			email.Recipient = customer.Email;
			return email;
		}
	}
}