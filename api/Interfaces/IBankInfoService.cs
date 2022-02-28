using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	/// <summary>
	/// Interface <c>IBankInfoService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para pegar, salvar e deletar os dados do banco de dados.
	/// E tambem prepara o email para enviar.
	/// </summary>
	public interface IBankInfoService
	{
		/// <summary>
		/// Este método <c>GetAllAsync</c> pega do banco de dados todos os os dados bancários.
		/// Em modo Async.
		/// </summary>
		/// <returns> Uma lista com todos os dados bancários salvo no banco de dados </returns>
		Task<List<BankInfo>> GetAllAsync();

		/// <summary>
		/// Este método <c>GetAsync</c> pega do banco de dados os dados bancário
		/// do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do customer pertencente aos dados bancários desejado. </param>
		/// <returns> Os dados bancários desejado. </returns>
		Task<BankInfo> GetAsync(int id);

		/// <summary>
		/// Este método <c>AddAsync</c> salva no banco de dados o novo
		/// <paramref name="bankInfo"/> recebido.
		/// Em modo Async.
		/// </summary>
		/// <param name="bankInfo"> Novos dados bancários. </param>
		/// <returns> O id do customer pertencente aos dados bancários no banco de dados.</returns>
		Task<int> AddAsync(BankInfo bankinfo);

		/// <summary>
		/// Este método <c>DeleteAsync</c> deleta do banco de dados os
		/// dados bancários do <paramref name="id"/> especificado pertencente ao customer.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do customer pertencente aos dados bancários a ser deletado.</param>
		/// <returns> True se conseguir deletar. </returns>
		/// <returns> False se não existir.</returns>
		Task<bool> DeleteAsync(int id);

		/// <summary>
		/// Este método <c>PrepareEmail</c> cria e popula uma nova instância do <c>Email</c>.
		/// O <c>Email</c> é populado com os dados do <paramref name="customer"/> recebido.
		/// </summary>
		/// <param name="customer">Instância do customer a ser usada.</param>
		/// <param name="hash">Hash unico criado para o customer.</param>
		/// <returns>Uma instância do Email populada.</returns>
		Email PrepareEmail(Customer customer, string hash);
	}
}