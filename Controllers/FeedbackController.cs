using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.Interfaces;
using backend.Data;

namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class FeedbackController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IFeedbackService _feedbackService;

		public FeedbackController(SellerContext context, IFeedbackService feedbackService)
		{
			_dbContext = context;
			_feedbackService = feedbackService;
		}

		// GET all action
		/// <summary>
		/// Solicita a lista de todos os feedback
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Feedback>> GetAll()
		{
			return (_feedbackService.GetAll());
		}

		// GET all action
		/// <summary>
		/// Solicita o feedback por id
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public ActionResult<Feedback> GetActionResult(int id)
		{
			var feedback = _feedbackService.Get(id);

			if (feedback == null)
				return NotFound();
			return feedback;
		}

		[HttpPost]
		[ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public IActionResult Create(Feedback feedback)
		{
			int feedbackId = _feedbackService.Add(feedback);
			// To-do: Should be async, or use a queue
			feedback.Id = feedbackId;
			return CreatedAtAction(nameof(Create), new { id = feedback.Id }, feedback);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public IActionResult Delete(int id)
		{
			if (_feedbackService.Delete(id) == false)
				return NotFound();
			return Ok();
		}
	}
}