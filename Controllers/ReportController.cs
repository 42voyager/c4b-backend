using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using Microsoft.Extensions.Configuration;


namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class ReportController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly ICustomerService _customerService;
		private readonly IConfiguration _configuration;

		// private readonly IEmailService<BankInfo> _emailService;

		public ReportController(
			SellerContext context,
			ICustomerService customerService,
			IConfiguration configuration
		)
		{
			_dbContext = context;
			_customerService = customerService;	
			_configuration = configuration;
		}

		// GET all action
		/// <summary>
		/// Solicita a lista de todos os Users con o status
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<CustomerReport>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<CustomerReport>>> GetAll()
		{
			var CustomersReport = await _customerService.GetAllReportAsync();
			return CustomersReport;
		}
	}
}