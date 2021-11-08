using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.Data;

namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly SellerContext dbContext;
		private readonly CustomerService customerService;

		public CustomerController(SellerContext context)
		{
			dbContext = context;
			customerService = new CustomerService(dbContext);
		}

		[HttpGet]
		public ActionResult<List<Customer>> GetAll()
		{
			return (customerService.GetAll());
		}

		[HttpGet("{id}")]
		public ActionResult<Customer> GetActionResult(int id)
		{
			var customer = customerService.Get(id);

			if (customer == null)
				return NotFound();
			return customer;
		}

		[HttpPost]
		public IActionResult Create(Customer customer)
		{
			customerService.Add(customer);
			return CreatedAtAction(nameof(Create), new { id = customer.Id }, customer);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Customer customer)
		{
			if (id != customer.Id)
				return BadRequest();
			var existingCustomer = customerService.Get(id);
			if (existingCustomer == null)
				return NotFound();
			customerService.Update(customer);

			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			if (customerService.Delete(id) == false)
				return NotFound();
			return Ok();
		}
	}
}