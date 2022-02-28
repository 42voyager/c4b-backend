using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
	/// <summary>
	/// Interface <c>IContractService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para pegar, salvar, atualizar e deletar os dados do contrato do banco de dados.
	/// </summary>
	public interface IContractService
	{

		/// <summary>
		/// Este método <c>GetAllAsync</c> pega do banco de dados todos os contratos.
		/// Em modo Async.
		/// </summary>
		/// <returns> Uma lista com todos os contratos salvo no banco de dados </returns>
		Task<List<Contract>> GetAllAsync();

		/// <summary>
		/// Este método <c>GetAsync</c> pega do banco de dados o contrato
		/// do id do customer especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do customer do contrato desejado. </param>
		/// <returns> O contrato desejado. </returns>
		Task<Contract> GetAsync(int id);

		/// <summary>
		/// Este método <c>AddAsync</c> salva no banco de dados o novo
		/// <paramref name="newContract"/> recebido.
		/// Em modo Async.
		/// </summary>
		/// <param name="newContract"> Novo contrato. </param>
		/// <returns> O id do customer do contrato no banco de dados. </returns>
		Task<int> AddAsync(Contract newContract);

		/// <summary>
		/// Este método <c>UpdateAsync</c> atualiza do banco de dados o
		/// <paramref name="updateContract"/> passado.
		/// Em modo Assync
		/// </summary>
		/// <param name="updateContract">Contrato a ser atualizado.</param>
		/// <returns>True se conseguir atualizar.</returns>
		/// <returns>False se o contrato não existir.</returns>
		Task<bool> UpdateAsync(Contract contract);

		/// <summary>
		/// Este método <c>DeleteAsync</c> deleta do banco de dados o contrato
		/// do <paramref name="id"/> especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do customer do contrato a ser deletado.</param>
		/// <returns> True se conseguir deletar. </returns>
		/// <returns> False se não existir.</returns>
		Task<bool> DeleteAsync(int id);
	}
}