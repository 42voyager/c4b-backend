using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Interfaces;

namespace backend.Controllers
{
	/// <summary>
	/// Classe <c>CreditController</c> herda <c>ControllerBase</c> controla os
	/// redirecionamentos da API para calcular o faturamento recomendado mostrado
	/// no formulário de solicitação do crédito.
	/// </summary>
	[ApiController]
	[Route("[Controller]")]
	public class CreditController : ControllerBase
	{
		private readonly ICreditService _creditService;
		public CreditController(
			ICreditService creditService
		)
		{
			_creditService = creditService;
		}

		/// <summary>
		/// Método <c>Get</c> pega os valores para fazer o cálculo do faturamento
		/// </summary>
		/// <param name="credit">Passa o objeto credit com a quantidade a ser
		/// solicitada e o número de parcelas</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		/// <returns>Retorna o faturamento recomendado calculado.</returns>
		[HttpPost]
		[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public ActionResult<double> Get(Credit credit)
		{
			var Income = _creditService.CalculateIncome(credit);
			return (Income);
		}
	}
}