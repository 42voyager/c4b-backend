using backend.Models;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	public interface IContractService
	{
		Task<BankInfo> GetBankInfoAsync(int id);
		Task<Customer> GetCustomerInfoAsync(int id);
		Task<Contract> GetAsync(int id);
		Task<int> AddAsync(Contract newContract);
		Task<bool> DeleteAsync(int id);
	}
}