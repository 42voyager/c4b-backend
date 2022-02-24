using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{

	/// <summary>
	/// Interface <c>ICustomerService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para pegar, salvar, atualizar e deletar os dados do banco de dados.
	/// E tambem prepara o email para enviar.
	/// Tambem posibilita pegar e atualizar os status do customer.
	/// </summary>
	public interface ICustomerService
	{

		/// <summary>
		/// Este método <c>GetAllAsync</c> pega do banco de dados todos os customer.
		/// Em modo Async.
		/// </summary>
		/// <returns> Uma lista com todos os customer salvo no banco de dados </returns>
		Task<List<Customer>> GetAllAsync();

		/// <summary>
		/// Este método <c>GetAllReportAsync</c> pega do banco de dados todos
		/// os relatórios dos customer.
		/// </summary>
		/// <returns>Uma lista de relatório de customers.</returns>
		Task<List<CustomerReport>> GetAllReportAsync();

		/// <summary>
		/// Este método <c>GetAsync</c> pega do banco de dados o customer
		/// do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do customer desejado. </param>
		/// <returns> O customer desejado. </returns>
		Task<Customer> GetAsync(int id);

		/// <summary>
		/// Este método <c>AddAsync</c> salva no banco de dados o novo
		/// <paramref name="newCustomer"/> recebido.
		/// Em modo Async.
		/// </summary>
		/// <param name="newCustomer"> Novo customer. </param>
		/// <returns> O id do customer no banco de dados. </returns>
		Task<int> AddAsync(Customer newCustomer);

		/// <summary>
		/// Este método <c>DeleteAsync</c> deleta do banco de dados o customer
		/// do <paramref name="id"/> especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do customer a ser deletado.</param>
		/// <returns> True se conseguir deletar. </returns>
		/// <returns> False se não existir.</returns>
		Task<bool> DeleteAsync(int id);

		/// <summary>
		/// Este método <c>UpdateAsync</c> atualiza do banco de dados o
		/// <paramref name="updateCustomer"/> passado.
		/// Em modo Assync
		/// </summary>
		/// <param name="updateCustomer">Customer a ser atualizado.</param>
		/// <returns>True se conseguir atualizar.</returns>
		/// <returns>False se o customer não existir.</returns>
		Task<bool> UpdateAsync(Customer updateCustomer);

		/// <summary>
		/// Este método <c>UpdateStatusAsync</c> atualiza no banco de dados o
		/// <paramref name="status"/> do customer do <paramref name="id"/> especificado.
		/// </summary>
		/// <param name="id">Id do customer.</param>
		/// <param name="status">Novo status.</param>
		/// <returns>True se conseguir atualizar.</returns>
		/// <returns>False se o customer não existir.</returns>
		Task<bool> UpdateStatusAsync(int id, string status);

		/// <summary>
		/// Este método <c>PrepareEmail</c> cria e popula uma nova instância do <c>Email</c>.
		/// O <c>Email</c> é populado com os dados do <paramref name="customer"/> recebido.
		/// O <c>Email</c> tambem contem um json com os dados.
		/// Em modo Async.
		/// </summary>
		/// <param name="customer">Instância do customer a ser usada.</param>
		/// <returns>Uma instância do Email populada.</returns>
		Task<Email> PrepareEmail(Customer customer);
	}
}