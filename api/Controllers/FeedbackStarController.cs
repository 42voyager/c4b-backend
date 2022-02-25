using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using System.Threading.Tasks;

namespace backend.Controllers
{
	/// <summary>
	/// Classe <c>FeedbackStartController</c> herda <c>ControllerBase</c> controla os
	/// redirecionamentos da API relacionados ao envio do Feedback sobre a avaliacao
	/// do produto
	/// </summary>
	[ApiController]
	[Route("[Controller]")]
	public class FeedbackStarController : ControllerBase
	{
		private readonly SellerContext _dbContext;
		private readonly IFeedbackStarService _feedbackStarService;

		public FeedbackStarController(
			SellerContext context,
			IFeedbackStarService feedbackStarService
			)
		{
			_dbContext = context;
			_feedbackStarService = feedbackStarService;
		}

		/// <summary>
		/// Metodo <c> GetAll </c> Solicita a lista de todos os feedbacks no sistema
		/// </summary>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet]
		[ProducesResponseType(typeof(List<FeedbackStar>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<FeedbackStar>>> GetAll()
		{
			return (await _feedbackStarService.GetAllAsync());
		}

		/// <summary>
		/// Metodo <c> GetActionResult </c> Solicita o feedbackStar por id
		/// </summary>
		/// <param name="id"> id do feedback object </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(List<FeedbackStar>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<FeedbackStar>> GetActionResult(int id)
		{
			var feedbackStar = await _feedbackStarService.GetAsync(id);

			if (feedbackStar == null)
				return NotFound();
			return feedbackStar;
		}

		/// <summary>
		/// Metodo <c> Create </c> Crea um novo feedback
		/// </summary>
		/// <param name="feedbackStart"> Objeto com a informacao do feedback </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpPost]
		[ProducesResponseType(typeof(FeedbackStar), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create(FeedbackStar feedbackStar)
		{
			var feedbackStarId = _feedbackStarService.AddAsync(feedbackStar);
			feedbackStar.Id = await feedbackStarId;
			return CreatedAtAction(nameof(Create), new { id = feedbackStar.Id }, feedbackStar);
		}

		/// <summary>
		/// Metodo <c> Delete </c> Deleta o feedback
		/// </summary>
		/// <param name="id"> id do feedback object </param>
		/// <response code="200"> Se tudo estiver correto </response>
		/// <response code="500"> Se ocorrerem erros de processamento no servidor </response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete(int id)
		{
			bool feedbackStar = await _feedbackStarService.DeleteAsync(id);
			if ( feedbackStar == false)
				return NotFound();
			return Ok();
		}

	}
}