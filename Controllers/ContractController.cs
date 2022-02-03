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
	[ApiController]
	[Route("[Controller]")]
	public class ContractController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IContractService _contractService;
		private readonly IRecaptchaService _recaptchaService;
		private readonly ICreatePdfService _createPdfService;
		private readonly IConfiguration _configuration;

		// private readonly IEmailService<BankInfo> _emailService;

		public ContractController(
			SellerContext context,
			IContractService contractService,
			ICreatePdfService createPdfService,
			IRecaptchaService recaptchaService,
			IConfiguration configuration
			// IEmailService<BankInfo> emailService
		)
		{
			_dbContext = context;
			_contractService = contractService;
			_createPdfService = createPdfService;
			_recaptchaService = recaptchaService;
			_configuration = configuration;
			// _emailService = emailService;
		}

		// GET all action
		/// <summary>
		/// Solicita a lista de todos os Contratos
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

		// GET by Id action
		/// <summary>
		/// Solicita o customer por id
		/// </summary>
		/// <param name="id"> id do customer </param>
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


		// Update contracto action
		/// <summary>
		/// Atualiza o contrato com a assinatura
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
			await _contractService.UpdateAsync(contract);
			return Ok();
		}

		// Delete contracto action
		/// <summary>
		/// Apaga um contrato linkado com a id do customer
		/// </summary>
		/// <param name="id"> id do usuário </param>
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