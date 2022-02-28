using System.Threading.Tasks;

namespace backend.Interfaces
{

	/// <summary>
	/// Interface <c>IRecaptchaService</c> tem os serviços que auxiliam
	/// os controllers nas requisições.
	/// Valida o recaptcha.
	/// </summary>
	public interface IRecaptchaService
	{

		/// <summary>
		/// Este método <c>ValidateRecaptchaScore</c> pega o <paramref name="token"/>
		/// e faz uma requisição para o serviço de Recaptcha.
		/// Após a requisição o toten é validado.
		/// </summary>
		/// <param name="token">Token do recaptcha para validação</param>
		/// <returns>True se o score for maior ou igual que o minimo necessário. </returns>
		/// <returns>False se o score for menor.</returns>
		Task<bool> ValidateRecaptchaScore(string token);
	}
}