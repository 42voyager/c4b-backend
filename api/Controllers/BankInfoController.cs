using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using backend.Services;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
/// <summary>
/// Classe <c>BankInfoController</c> herda <c>ControllerBase</c> controla os
/// direcionamentos da API relacionados ao cadastro dos dados bancários no
/// sistema depois da solicitação inicial ter sido aprovada.
/// </summary>
	[ApiController]
	[Route("[Controller]")]
	public class BankInfoController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IBankInfoService _bankInfoService;
		private readonly ICustomerService _customerService;
		private readonly IRecaptchaService _recaptchaService;
		private readonly ICreatePdfService _createPdfService;
		private readonly IConfiguration _configuration;

		public BankInfoController(
			SellerContext context,
			IBankInfoService bankInfoService,
			ICustomerService customerService,
			IRecaptchaService recaptchaService,
			ICreatePdfService createPdfService,
			IConfiguration configuration
		)
		{
			_dbContext = context;
			_bankInfoService = bankInfoService;
			_customerService = customerService;
			_recaptchaService = recaptchaService;
			_createPdfService = createPdfService;
			_configuration = configuration;
		}

		/// <summary>
		/// Método <c>GetAll</c> solicita a lista de todos os dados bancários
		/// cadastrados no sistema.
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no
		/// servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<BankInfo>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<BankInfo>>> GetAll()
		{
			return (await _bankInfoService.GetAllAsync());
		}

		/// <summary>
		/// Método <c>GetActionResult</c> solicita o bankInfo por id do customer
		/// </summary>
		/// <param name="hash">Hash único do link gerado para cadastrar os dados
		/// bancários</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		/// </summary>
		[HttpGet("{hash}")]
		[ProducesResponseType(typeof(List<BankInfo>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<BankInfo>> GetActionResult(string hash)
		{
			var id = HashService.GetIdFromHash(_configuration.GetSection("Aes:Key").Value, hash);
			if (id == -1)
				return NotFound();
			var bankInfo = await _bankInfoService.GetAsync(id);

			if (bankInfo == null)
				return NotFound();
			return bankInfo;
		}

		/// <summary>
		/// Método <c>Create</c> guarda os dados bancários do customer no banco
		/// de dados usando o ID do customer como referência.
		/// </summary>
		/// <param name="bankInfo">Modelo <c>BankInfoView</c> para guardar com os
		/// elementos da conta bancária.</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpPost]
		[ProducesResponseType(typeof(BankInfo), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create(BankInfoView bankInfo)
		{
			bool isHuman = await _recaptchaService.ValidateRecaptchaScore(bankInfo.RecaptchaToken);

			if (isHuman == true)
			{
				var customerID = HashService.GetIdFromHash(_configuration.GetSection("Aes:Key").Value, bankInfo.hash);
				bankInfo.CustomerID = customerID;
				var userId = await _bankInfoService.AddAsync(bankInfo);
				if (await _customerService.UpdateStatusAsync(userId, Status.BankInfo) == false)
					return NotFound();

				await _createPdfService.CreatePdf(bankInfo.CustomerID);
				return CreatedAtAction(nameof(Create), new { id = bankInfo.CustomerID }, bankInfo);
			}
			else
				return new StatusCodeResult(429);
		}

		/// <summary>
		/// Método <c>Delete</c> apaga os dados bancários do ID de customer especificado.
		/// </summary>
		/// <param name="id">ID do customer</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="404"> Se o ID não foi encontrado </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete(int id)
		{
			bool bankInfo = await _bankInfoService.DeleteAsync(id);
			if ( bankInfo == false)
				return NotFound();
			return Ok();
		}

	}
}