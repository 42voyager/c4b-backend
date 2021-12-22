using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using backend.Models;
using backend.Data;
using backend.Interfaces;
using System;
using System.Threading.Tasks;
using System.IO;

namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly ICustomerService _customerService;
		private readonly IEmailService<Customer> _emailService;
		private readonly IRecaptchaService _recaptchaService;

		public CustomerController(
			SellerContext context,
			ICustomerService customerService,
			IEmailService<Customer> emailService,
			IRecaptchaService recaptchaService
			)
		{
			_dbContext = context;
			_customerService = customerService;
			_emailService = emailService;
			_recaptchaService = recaptchaService;
		}

		// GET all action
		/// <summary>
		/// Solicita a lista de todos os customers
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

		// GET by Id action
		/// <summary>
		/// Solicita o customer por id
		/// </summary>
		/// <param name="id"> id do customer </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Customer>> GetActionResult(int id)
		{
			var customer = await _customerService.GetAsync(id);

			if (customer == null)
				return NotFound();
			return customer;
		}

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
				var email = await prepareEmail(customer);
				var send = _emailService.SendEmailAsync(email);
				await Task.WhenAll(userId);
				return CreatedAtAction(nameof(Create), new { id = customer.Id }, customer);
			}
			else
				return new StatusCodeResult(429);
		}

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

		private async Task<Email> prepareEmail(Customer customer)
		{
			DateTime localDate = DateTime.Now;
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsonDataCustomer-{customer.Id}.json";
			var email = new Email();

			await _emailService.PrepareCustomerJsonAsync(customer, attachmentPath);
			email.AttachmentPath = attachmentPath;
			email.Subject = $"Nova solicitação de crédito: Empresa {customer.Company}";
			email.Body = string.Format(
					@$"<div style='max-width: 100%; width: calc(100% - 60px); padding: 30px 30px; text-align: center;'>
					<h1 style='font-size= 14px; '>Nova Solicitação <br>{localDate.ToString()}</h1> 
					<p>Nova solicitação de crédito, feita pela empresa {customer.Company}</p>
					<p>Todas as informações disponiveis estão guardadas no json anexado!</p>
					<p></p><br>
					<hr style='border: 2px solid #b29475;'>
  					<p style='padding: 10px; color: #b29475;'>Equipe Voyager.</p>
					</div>");
			return email;
		}
	}
}