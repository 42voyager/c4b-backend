using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class FeedbackController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IFeedbackService _feedbackService;
		private readonly IEmailService<Feedback> _emailService;
		private readonly IRecaptchaService _recaptchaService;
		private readonly IConfiguration _configuration;


		public FeedbackController(
			SellerContext context,
			IFeedbackService feedbackService,
			IEmailService<Feedback> emailService,
			IRecaptchaService recaptchaService,
			IConfiguration configuration
			)
		{
			_dbContext = context;
			_feedbackService = feedbackService;
			_emailService = emailService;
			_recaptchaService = recaptchaService;
			_configuration = configuration;
		}

		/// <summary>
		/// Método <c>GetAll</c> solicita a lista de todos os feedbacks feitos
		/// na landing page com seus atributos.
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<Feedback>>> GetAll()
		{
			return (await _feedbackService.GetAllAsync());
		}

		/// <summary>
		/// Método asíncrono <c>GetActionResult</c> solicita um feedback pelo id
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Feedback>> GetActionResult(int id)
		{
			var feedback = await _feedbackService.GetAsync(id);

			if (feedback == null)
				return NotFound();
			return feedback;
		}

		/// <summary>
		/// Método <c>Create</c> guarda um feedback no banco de dados e envia um
		/// email para o administrador
		/// </summary>
		/// <param name="feedback">Um objeto feedback que contem nome, email e
		/// mensagem</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="429"> Se o servidor não conseguir responder a solicitação
		/// por estar sobrecarregado</response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpPost]
		[ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status429TooManyRequests)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create(FeedbackView feedback)
		{
			bool isHuman = await _recaptchaService.ValidateRecaptchaScore(feedback.RecaptchaToken);

			if (isHuman == true)
			{
				var feedbackId = _feedbackService.AddAsync(feedback);
				feedback.Id = await feedbackId;
				var email = await PrepareEmailFeedback(feedback);
				var send = _emailService.SendEmailAsync(email);
				await Task.WhenAll(feedbackId);
				return CreatedAtAction(nameof(Create), new { id = feedback.Id }, feedback);
			}
			else
				return new StatusCodeResult(429);
		}

		/// <summary>
		/// Método asíncrono <c>Delete</c> deleta um feedback do banco de dados usando
		/// a id dele.
		/// </summary>
		/// <param name="id">ID do feedback a ser deletado.</param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="404"> Se a ID do feedback não existe</response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete(int id)
		{
			bool feedback = await _feedbackService.DeleteAsync(id);
			if ( feedback == false)
				return NotFound();
			return Ok();
		}

		private async Task<Email> PrepareEmailFeedback(Feedback feedback)
		{
			DateTime localDate = DateTime.Now;
			string attachmentPath = Directory.GetCurrentDirectory() + $"/JsonData/jsonDataFeedback-{feedback.Id}.json";
			var email = new Email();

			// cria o caminho do arquivo json que será anexado ao email
			await _emailService.PrepareJsonAsync(feedback, attachmentPath);
			email.AttachmentPath = attachmentPath;
			string[] templateBody = _configuration.GetSection("EmailTemplates:EmailFeedback:Body").Get<string[]>();
			email.Subject = string.Format(
				_configuration.GetSection("EmailTemplates:EmailFeedback:Subject").Value,
				feedback.Name
			);
			email.Body = string.Format(
				String.Join(" ", templateBody),
				localDate.ToLongDateString(),
				feedback.Name,
				feedback.Email,
				feedback.Message
			);
			return email;
		}
	}
}