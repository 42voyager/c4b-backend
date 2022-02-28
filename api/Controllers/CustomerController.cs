using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using backend.Models;
using backend.Data;
using backend.Interfaces;
using backend.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
	/// <summary>
	/// Classe <c>CustomerController</c> herda <c>ControllerBase</c> controla os
	/// redirecionamentos da API relacionados ao cadastro de customers dentro
	/// do sistema
	/// </summary>
	[ApiController]
	[Route("[Controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly ICustomerService _customerService;
		private readonly IEmailService<Customer> _emailService;
		private readonly IRecaptchaService _recaptchaService;
		private readonly IConfiguration _configuration;

		private readonly IBankInfoService _bankInfoService;

		public CustomerController(
			SellerContext context,
			ICustomerService customerService,
			IEmailService<Customer> emailService,
			IRecaptchaService recaptchaService,
			IConfiguration configuration,
			IBankInfoService bankInfoService
			)
		{
			_dbContext = context;
			_customerService = customerService;
			_emailService = emailService;
			_recaptchaService = recaptchaService;
			_configuration = configuration;
			_bankInfoService = bankInfoService;
		}

		/// <summary>
		/// Método <c>GetAll</c> solicita a lista de todos os customers cadastrados
		/// no sistema com seus atributos
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<Customer>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<Customer>>> GetAll()
		{
			return ( await _customerService.GetAllAsync());
		}

		/// <summary>
		/// Método asíncrono <c>GetActionResult</c> solicita o customer por id
		/// </summary>
		/// <param name="hash"> Has da URL para extraer a ID do customer </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		/// <returns>Retorna o customer se ele existe.</returns>
		[HttpGet("{hash}")]
		[ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Customer>> GetActionResult(string hash)
		{
			// id é extraído do hash da URL
			var id = HashService.GetIdFromHash(_configuration.GetSection("Aes:Key").Value, hash);
			if (id == -1)
				return NotFound();
			var customer = await _customerService.GetAsync(id);
			if (customer == null)
				return NotFound();
			return customer;
		}

		/// <summary>
		/// Método <c>Create</c> cria um customer dentro do sistema e guarda no
		/// banco de dados. Envia um email para o customer avisando que a solicitação
		/// foi feita e um email com a solicitação e o arquivo json dos dados do
		/// customer para o administrador do sistema.
		/// </summary>
		/// <param name="customer">Objeto customer com seus atributos</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="429"> Se o servidor não conseguir responder a solicitação
		/// por estar sobrecarregado</response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpPost]
		[ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(object), StatusCodes.Status429TooManyRequests)]
		public async Task<IActionResult> Create(CustomerView customer)
		{
			bool isHuman = await _recaptchaService.ValidateRecaptchaScore(customer.RecaptchaToken);

			if (isHuman == true)
			{
				var userId =  _customerService.AddAsync(customer);
				customer.Id = await userId;
				string cryptedUserId = HashService.EncryptString(
					_configuration.GetSection("Aes:Key").Value, 
					customer.Id.ToString() + customer.Cnpj
				);

				// Email enviado para informar ao administrador do sistema a
				// solicitação do customer
				var emailToBank = await _customerService.PrepareEmail(customer);
				var sendBank = _emailService.SendEmailAsync(emailToBank);

				// Email enviado ao customer para solicitar os dados bancários
				var emailToCustomer = _bankInfoService.PrepareEmail(customer, cryptedUserId);
				var sendCustomer = _emailService.SendEmailAsync(emailToCustomer);

				await Task.WhenAll(userId);
				return CreatedAtAction(nameof(Create), new { id = customer.Id }, customer);
			}
			else
				return new StatusCodeResult(429);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="id">ID do customer</param>
		/// <param name="customer">Objeto do customer com todos os atributos</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="400"> Se não há uma ID na solicitação</response>
		/// <response code="404"> Se a ID do usuário não existe</response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Update(int id, Customer customer)
		{
			if (id != customer.Id)
				return BadRequest();
			var existingCustomer = await _customerService.GetAsync(id);
			if (existingCustomer == null)
				return NotFound();
			await _customerService.UpdateAsync(customer);
			return Ok();
		}

		/// <summary>
		/// Método <c>Delete</c> apaga o customer do banco de dados
		/// </summary>
		/// <param name="id">ID do customer</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="404"> Se a ID do usuário não existe</response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete(int id)
		{
			bool customer = await _customerService.DeleteAsync(id);
			if (customer == false)
				return NotFound();
			return Ok();
		}
	}
}