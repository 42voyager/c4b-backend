using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace backend.Services
{

	/// <summary>
	/// Class <c>ContractService</c> implementa <c>IContractService</c> interface.
	/// </summary>
	public class ContractService : IContractService
	{
		private readonly SellerContext _dbContext;

		/// <summary>
		/// Este construtor inicializa o <paramref name="context"/> do banco de dados.
		/// </summary>
		/// <param name="context">Instância do banco de dados.</param>
		public ContractService(SellerContext context)
		{
			_dbContext = context;
		}

		/// <inheritdoc />
		public async Task<List<Contract>> GetAllAsync()
		{
			return await _dbContext.Contracts.ToListAsync();
		}

		/// <inheritdoc />
		public async Task<Contract> GetAsync(int id)
		{
			return await _dbContext.Contracts.FirstOrDefaultAsync(p => p.CustomerID == id);
		}

		/// <inheritdoc />
		public async Task<int> AddAsync(Contract newContract)
		{
			var result = await _dbContext.Contracts.AddAsync(newContract);
			await _dbContext.SaveChangesAsync();
			return result.Entity.CustomerID;
		}

		/// <inheritdoc />
		public async Task<bool> UpdateAsync(Contract updateContract)
		{
			var contract = await _dbContext.Contracts
				.FirstOrDefaultAsync(p => p.CustomerID == updateContract.CustomerID);
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

		/// <inheritdoc />
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