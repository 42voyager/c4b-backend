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
	public class BankInfoController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IBankInfoService _bankInfoService;
		private readonly IRecaptchaService _recaptchaService;

		// private readonly IEmailService<BankInfo> _emailService;

		public BankInfoController(
			SellerContext context,
			IBankInfoService bankInfoService,
			IRecaptchaService recaptchaService
			// IEmailService<BankInfo> emailService
		)
		{
			_dbContext = context;
			_bankInfoService = bankInfoService;
			_recaptchaService = recaptchaService;
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
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(List<BankInfo>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<BankInfo>> GetActionResult(int id)
		{
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
				var bankInfoId = _bankInfoService.AddAsync(bankInfo);
				bankInfo.Id = await bankInfoId;
				// var email = await PrepareEmailbankInfo(bankInfo);
				// var send = _emailService.SendEmailAsync(email);
				await Task.WhenAll(bankInfoId);
				return CreatedAtAction(nameof(Create), new { id = bankInfo.Id }, bankInfo);
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