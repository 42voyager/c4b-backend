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
	[ApiController]
	[Route("[Controller]")]
	public class BankInfoController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IBankInfoService _bankInfoService;
		private readonly IRecaptchaService _recaptchaService;
		private readonly ICreatePdfService _createPdfService;
		private readonly IConfiguration _configuration;

		// private readonly IEmailService<BankInfo> _emailService;

		public BankInfoController(
			SellerContext context,
			IBankInfoService bankInfoService,
			IRecaptchaService recaptchaService,
			ICreatePdfService createPdfService,
			IConfiguration configuration
			// IEmailService<BankInfo> emailService
		)
		{
			_dbContext = context;
			_bankInfoService = bankInfoService;
			_recaptchaService = recaptchaService;
			_createPdfService = createPdfService;
			_configuration = configuration;
			// _emailService = emailService;
		}

		// GET all action
		/// <summary>
		/// Solicita a lista de todos os bankAccounts
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<BankInfo>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<BankInfo>>> GetAll()
		{
			return (await _bankInfoService.GetAllAsync());
		}

		// GET all action
		/// <summary>
		/// Solicita o bankInfo por id
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
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
				var bankInfoId = _bankInfoService.AddAsync(bankInfo);
				// var email = await PrepareEmailbankInfo(bankInfo);
				// var send = _emailService.SendEmailAsync(email);
				await Task.WhenAll(bankInfoId);

				await _createPdfService.CreatePdf(bankInfo.CustomerID);
				return CreatedAtAction(nameof(Create), new { id = bankInfo.CustomerID }, bankInfo);
			}
			else
				return new StatusCodeResult(429);
		}

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