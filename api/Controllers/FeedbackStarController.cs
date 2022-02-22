using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Interfaces;
using backend.Data;
using System.Threading.Tasks;

namespace backend.Controllers
{
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

		// GET all action
		/// <summary>
		/// Solicita a lista de todos os feedbackStar
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

		// GET all action
		/// <summary>
		/// Solicita o feedbackStar por id
		/// </summary>
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

		[HttpPost]
		[ProducesResponseType(typeof(FeedbackStar), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create(FeedbackStar feedbackStar)
		{
			var feedbackStarId = _feedbackStarService.AddAsync(feedbackStar);
			feedbackStar.Id = await feedbackStarId;
			return CreatedAtAction(nameof(Create), new { id = feedbackStar.Id }, feedbackStar);
		}

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