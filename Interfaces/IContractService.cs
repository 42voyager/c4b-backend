using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	public interface IContractService
	{
		Task<List<Contract>> GetAllAsync();
		Task<Contract> GetAsync(int id);
		Task<int> AddAsync(Contract newContract);
		Task<bool> UpdateAsync(Contract contract);
		Task<bool> DeleteAsync(int id);
	}
}