using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{

	/// <summary>
	/// Interface <c>IFeedbackStarService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para pegar, salvar e deletar os feedback com estrelas do banco de dados.
	/// </summary>
	public interface IFeedbackStarService
	{

		/// <summary>
		/// Este método <c>GetAllAsync</c> pega do banco de dados todos os
		/// feedback com estrela.
		/// Em modo Async.
		/// </summary>
		/// <returns> Uma lista com todos os feedback de
		/// estrela salvo no banco de dados </returns>
		Task<List<FeedbackStar>> GetAllAsync();

		/// <summary>
		/// Este método <c>GetAsync</c> pega do banco de dados o feedback
		/// com estrela do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do feedback desejado. </param>
		/// <returns> O feedback desejado. </returns>
		Task<FeedbackStar> GetAsync(int id);

		/// <summary>
		/// Este método <c>AddAsync</c> salva no banco de dados o novo feedback recebido.
		/// Em modo Async.
		/// </summary>
		/// <param name="newFeedback"> Novo feedback com estrela. </param>
		/// <returns> O id do feedback no banco de dados. </returns>
		Task<int> AddAsync(FeedbackStar feedbackStar);

		/// <summary>
		/// Este método <c>DeleteAsync</c> deleta do banco de dados o feedback
		/// com estrela do id especificado.
		/// Em modo Async.
		/// </summary>
		/// <param name="id"> Id do feedback de estrela a ser deletado.</param>
		/// <returns> True se conseguir deletar. </returns>
		/// <returns> False se não existir.</returns>
		Task<bool> DeleteAsync(int id);
	}
}