using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using System.Collections.Generic;
using backend.Services;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
	/// <summary>
	/// Classe <c>ContractController</c> herda <c>ControllerBase</c> controla os
	/// direcionamentos da API relacionados à assinatura e aceitação dos termos
	/// do contrato.
	/// </summary>
	[ApiController]
	[Route("[Controller]")]
	public class ContractController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IContractService _contractService;
		private readonly ICustomerService _customerService;
		private readonly IRecaptchaService _recaptchaService;
		private readonly ICreatePdfService _createPdfService;
		private readonly IConfiguration _configuration;

		public ContractController(
			SellerContext context,
			IContractService contractService,
			ICreatePdfService createPdfService,
			IRecaptchaService recaptchaService,
			IConfiguration configuration,
			ICustomerService customerService
		)
		{
			_dbContext = context;
			_contractService = contractService;
			_createPdfService = createPdfService;
			_recaptchaService = recaptchaService;
			_configuration = configuration;
			_customerService = customerService;
		}

		/// <summary>
		/// Método asíncrono <c>GetAllAsync</c> solicita a lista de todos os
		/// contratos criados no banco de dados, assinados ou não.
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<Contract>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<Contract>>> GetAllAsync()
		{
			return await _contractService.GetAllAsync();
		}

		/// <summary>
		/// Método <c>GetActionResult</c> verifica se existe o contrato solicitado
		/// pela URL
		/// </summary>
		/// <param name="hash"> Hash do link da URL para assinar o contrato </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet("{hash}")]
		[ProducesResponseType(typeof(Contract), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Contract>> GetActionResult(string hash)
		{
			var id = HashService.GetIdFromHash(_configuration.GetSection("Aes:Key").Value, hash);
			if (id == -1)
				return NotFound();
			var contract = await _contractService.GetAsync(id);

			if (contract == null)
				return NotFound();
			return contract;
		}

		/// <summary>
		/// Método asíncrono <c>Update</c> atualiza o contrato com a assinatura
		/// e aceitação dos termos
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpPut]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Update(Contract contract)
		{
			var existingContract = await _contractService.GetAsync(contract.CustomerID);
			if (existingContract == null)
				return NotFound();
			if (contract.AcceptTerms && contract.AuthorizeSCR && contract.ExistsPEP)
			{
				if (await _customerService.UpdateStatusAsync(existingContract.CustomerID, Status.Contract) == false)
					return NotFound();
			}
			await _contractService.UpdateAsync(contract);
			return Ok();
		}

		/// <summary>
		/// Método asíncrono <c>Delete</c> apaga um contrato linkado com a id do
		/// customer
		/// </summary>
		/// <param name="id"> id do customer </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Contract>> Delete(int id)
		{
			bool result = await _contractService.DeleteAsync(id);
			if (result == false)
				return NotFound();
			return Ok();
		}

	}
}