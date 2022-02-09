using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using backend.Services;
using backend.Models;
using backend.Interfaces;
using backend.Data;

namespace backend.Controllers
{
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

		// GET
		/// <summary>
		/// Pega os valores para fazer o cálculo do faturamento
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
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