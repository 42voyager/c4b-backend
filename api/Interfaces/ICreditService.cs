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

		/// <summary>
		/// Este método <c>AproximateToRange</c> pega o <paramref name="input"/>
		/// e o <paramref name="range"/> e faz o calculo para deixar o <paramref name="input"/>
		/// mais próximo do <paramref name="range"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		double AproximateToRange(double input, double range);

	}
}