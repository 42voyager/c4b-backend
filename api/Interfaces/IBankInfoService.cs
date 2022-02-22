using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	public interface IBankInfoService
	{
		Task<List<BankInfo>> GetAllAsync();
		Task<int> AddAsync(BankInfo bankinfo);
		Task<BankInfo> GetAsync(int id);
		Task<bool> DeleteAsync(int id);

		Email PrepareEmail(Customer customer, string hash);
	}
}