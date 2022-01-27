using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class ContractService : IContractService
	{
		private readonly SellerContext _dbContext;

		public ContractService(SellerContext context)
		{
			_dbContext = context;
		}
		public async Task<Customer> GetCustomerInfoAsync(int id)
		{
			return await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
		}
		public async Task<BankInfo> GetBankInfoAsync(int id)
		{
			return await _dbContext.BankInfo.FirstOrDefaultAsync(p => p.CustomerID == id);
		}
		public async Task<Contract> GetAsync(int id)
		{
			return await _dbContext.Contracts.FirstOrDefaultAsync(p => p.CustomerID == id);
		}
		public async Task<int> AddAsync(Contract newContract)
		{
			var result = await _dbContext.Contracts.AddAsync(newContract);
			await _dbContext.SaveChangesAsync();
			return result.Entity.CustomerID;
		}
		public async Task<bool> DeleteAsync(int id)
		{
			var Contract = await _dbContext.Contracts.FindAsync(id);
			if (Contract == null)
				return false;
			_dbContext.Contracts.Remove(Contract);
			await _dbContext.SaveChangesAsync();
			return (true);
		}
	}
}