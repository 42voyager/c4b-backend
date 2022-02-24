using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{

	/// <summary>
	/// Interface <c>IFeedbackService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para pegar, salvar e deletar os feedback do banco de dados.
	/// </summary>
	public interface IFeedbackService
	{

		/// <summary>
		/// Este método <c>GetAllAsync</c> pega do banco de dados todos os
		/// feedback sem estrela.
		/// Em modo Async.
		/// </summary>
		/// <returns> Uma lista com todos os feedback sem estrelas
		/// salvo no banco de dados </returns>
		Task<List<Feedback>> GetAllAsync();

		/// <summary>
		/// Este método <c>GetAsync</c> pega do banco de dados o feedback
		/// sem estrela do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do feedback desejado. </param>
		/// <returns> O feedback desejado. </returns>
		Task<Feedback> GetAsync(int id);

		/// <summary>
		/// Este método <c>AddAsync</c> salva no banco de dados o novo feedback recebido.
		/// Em modo Async.
		/// </summary>
		/// <param name="newFeedback"> Novo feedback sem estrela. </param>
		/// <returns> O id do feedback no banco de dados. </returns>
		Task<int> AddAsync(Feedback newFeedback);

		/// <summary>
		/// Este método <c>DeleteAsync</c> deleta do banco de dados o feedback
		/// sem estrela do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do feedback de estrela a ser deletado.</param>
		/// <returns> True se conseguir deletar. </returns>
		/// <returns> False se não existir.</returns>
		Task<bool> DeleteAsync(int id);
	}
}