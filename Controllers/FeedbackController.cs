using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using System;
using System.Globalization;

namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class FeedbackController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IFeedbackService _feedbackService;
		private readonly IEmailService<Feedback> _emailService;

		public FeedbackController(SellerContext context, IFeedbackService feedbackService, IEmailService<Feedback> emailService)
		{
			_dbContext = context;
			_feedbackService = feedbackService;
			_emailService = emailService;
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
			var email = prepareEmail(feedback);
			_emailService.SendEmail(feedback, feedback.Id, email);
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

		private Email prepareEmail(Feedback feedback)
		{
			DateTime localDate = DateTime.Now;
			var email = new Email();

			email.Subject = $"Novo feedback no site C4B: {feedback.Name}";
			email.Body = string.Format(
					@$"<div style='background-color: #b29475; width: 100%; padding: 50px 30px; text-align: center;'>
					<h1 style='font-size= 14px; '>Pedido - {localDate.ToString()} <br>id - {feedback.Id}</h1> 
					<p>Novo feedback no site C4B , feito pelo usuario {feedback.Name}</p>
					<p>Todas as informações disponiveis estão guardadas no json anexado!</p>
					<p></p><br>
					<p style='margin: 40px; padding: 20px; background: white; color: #b29475;'>Equipe Voyager.</p>
					</div>");
			return email;
		}
	}
}