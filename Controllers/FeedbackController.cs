using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using System;
using System.IO;

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
			var email = PrepareEmailFeedback(feedback);
			_emailService.SendEmail(email);
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

		private Email PrepareEmailFeedback(Feedback feedback)
		{
			DateTime localDate = DateTime.Now;
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsonDataFeedback-{feedback.Id}.json";
			var email = new Email();
			
			// We create the path of json file that will be attached to the email
			_emailService.PrepareCustomerJson(feedback, attachmentPath);
			email.AttachmentPath = attachmentPath;
			email.Subject = $"Novo feedback no site C4B: {feedback.Name}";
			email.Body = string.Format(
					@$"<div style='max-width: 100%; width: calc(100% - 60px); padding: 30px 30px; text-align: center;'>
						<h1 style='font-size= 14px; '>Novo feedback <br> {localDate.ToString()}</h1><br>
						<div style='border: 1px solid #b29475; text-align: left; padding: 10px 25px'>
						<p style='border-bottom: 1px solid #b29475;'>Nome: {feedback.Name}</p>
						<p style='border-bottom: 1px solid #b29475;'>Email: {feedback.Email}</p>
						<p style='border-bottom: 1px solid #b29475;'>Mensagem:<br>{feedback.Message}</p>
						</div><br>
						<hr style='border: 2px solid #b29475;'>
						<p style='padding: 10px; color: #b29475;'>Equipe Voyager.</p>
					</div>");
			return email;
		}
	}
}