using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using backend.Services;
using backend.Models;
using backend.Data;
using backend.Interfaces;
using System;
using System.Globalization;

namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly ICustomerService _customerService;
		private readonly IEmailService<Customer> _emailService;

		public CustomerController(SellerContext context, ICustomerService customerService, IEmailService<Customer> emailService)
		{
			_dbContext = context;
			_customerService = customerService;
			_emailService = emailService;
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
		public ActionResult<List<Customer>> GetAll()
		{
			return (_customerService.GetAll());
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
		public ActionResult<Customer> GetActionResult(int id)
		{
			var customer = _customerService.Get(id);

			if (customer == null)
				return NotFound();
			return customer;
		}

		[HttpPost]
		[ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public IActionResult Create(Customer customer)
		{
			int userId = _customerService.Add(customer);
			// To-do: Should be async, or use a queue
			customer.Id = userId;
			var email = prepareEmail(customer);
			_emailService.SendEmail(customer, customer.Id, email);
			return CreatedAtAction(nameof(Create), new { id = customer.Id }, customer);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public IActionResult Update(int id, Customer customer)
		{
			if (id != customer.Id)
				return BadRequest();
			var existingCustomer = _customerService.Get(id);
			if (existingCustomer == null)
				return NotFound();
			_customerService.Update(customer);

			return Ok();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public IActionResult Delete(int id)
		{
			if (_customerService.Delete(id) == false)
				return NotFound();
			return Ok();
		}

		private Email prepareEmail(Customer customer)
		{
			DateTime localDate = DateTime.Now;
			var email = new Email();

			email.Subject = $"Nova solicitação de crédito: Empresa {customer.Company}";
			email.Body = string.Format(
					@$"<div style='background-color: #b29475; width: 100%; padding: 50px 30px; text-align: center;'>
					<h1 style='font-size= 14px; '>Pedido - {localDate.ToString()} <br>id - {customer.Id}</h1> 
					<p>Nova solicitação de crédito, feita pelo empresa {customer.Company}</p>
					<p>Todas as informações disponiveis estão guardadas no json anexado!</p>
					<p></p><br>
					<p style='margin: 40px; padding: 20px; background: white; color: #b29475;'>Equipe Voyager.</p>
					</div>");
			return email;
		}
	}
}