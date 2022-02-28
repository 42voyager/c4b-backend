using backend.Models;
namespace backend.Interfaces
{

	/// <summary>
	/// Interface <c>ICreditService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para calcular a renda entre o limite e parcelas do crédito.
	/// </summary>
	public interface ICreditService
	{
		/// <summary>
		/// Este método <c>CalculateIncome</c> calcula a renda do crédito
		/// usando o limite e as parcelas recebidas no <paramref name="credit"/>.
		/// </summary>
		/// <param name="credit">Intância da classe Credit contendo o limite e as parcelas.</param>
		/// <returns>O valor aproximado do calculo.</returns>
		double CalculateIncome(Credit credit);
	}
}