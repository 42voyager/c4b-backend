using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace backend.Services
{
	public class ContractService : IContractService
	{
		private readonly SellerContext _dbContext;

		public ContractService(SellerContext context)
		{
			_dbContext = context;
		}

		public async Task<List<Contract>> GetAllAsync()
		{
			return await _dbContext.Contracts.ToListAsync();
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
		public async Task<bool> UpdateAsync(Contract updateContract)
		{
			var contract = await _dbContext.Contracts.FirstOrDefaultAsync(p => p.CustomerID == updateContract.CustomerID);
			if (contract == null)
				return false;
			DateTime currentDate = DateTime.Now;
			contract.AcceptTerms = updateContract.AcceptTerms;
			contract.AuthorizeSCR = updateContract.AuthorizeSCR;
			contract.ExistsPEP = updateContract.ExistsPEP;
			contract.SignDate = currentDate;
			await _dbContext.SaveChangesAsync();
			return (true);
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