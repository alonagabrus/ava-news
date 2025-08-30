using System.ComponentModel.DataAnnotations;
using AvaTradeNews.Api.Models;
using AvaTradeNews.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace AvaTradeNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsQueryService _newsService;
        private readonly ILogger<NewsController> _logger;

        public NewsController(INewsQueryService newsService, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        /// <summary>
        /// Get all news articles.

        /// </summary>
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get all news articles", Description = "Returns the complete list of news articles.")]
        public async Task<ActionResult<List<NewsArticle>>> GetAll()
        {
            try
            {
                var news = await _newsService.GetAllAsync();
                return Ok(news);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access while trying to get all news");
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch all news.");
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                                title: "Unexpected error",
                                detail: ex.Message,
                                instance: HttpContext?.Request?.Path.Value);
            }
        }

        /// <summary>
        /// Get news from N previous days
        /// </summary>
        /// <param name="days">Number of days back to include.</param>
        [HttpGet("days/{days:int:range(0,365)}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get news by days back", Description = "Returns news articles from the last N days.")]
        public async Task<ActionResult<List<NewsArticle>>> GetFromLastNDays([FromRoute, Range(0, 365, ErrorMessage = "days must be between 0 and 365")] int days)
        {
            try
            {
                var news = await _newsService.GetFromLastNDaysAsync(days);
                return Ok(news);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access while trying to fetch news for last {Days} days.", days);
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch news for last {Days} days.", days);
                return Problem(detail: ex.Message,
                                statusCode: 500,
                                title: "Unexpected error",
                                instance: HttpContext?.Request?.Path.Value);
            }
        }

        /// <summary>
        /// Get news by instrument or ticker.
        /// </summary>
        /// <param name="instrument">Ticker or instrument identifier.</param>
        /// <param name="limit">Maximum items to return.</param>
        [HttpGet("instrument/{instrument}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get news by instrument", Description = "Returns news filtered by instrument or ticker( e.i. AAPL,NVDA)")]
        public async Task<ActionResult<List<NewsArticle>>> GetByInstrument(
            [FromRoute][Required][MinLength(1)] string instrument,
            [FromQuery][Range(1, 100)] int limit = 10)
        {
            try
            {
                var news = await _newsService.GetByInstrumentAsync(instrument, limit);
                return Ok(news);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetByInstrument - {Instrument}", instrument);
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch news for instrument {Instrument}.", instrument);
                return Problem(detail: ex.Message, statusCode: 500, title: "Unexpected error", instance: HttpContext?.Request?.Path.Value);
            }
        }

        /// <summary>
        /// Search news by free text.
        /// </summary>
        /// <param name="text">Search text.</param>
        [HttpGet("search")]
        [Authorize]
        [SwaggerOperation(Summary = "Search news", Description = "Full text search on news articles.(By title or content)")]
        public async Task<ActionResult<List<NewsArticle>>> SearchByText(
            [FromQuery][Required][MinLength(1)] string text)
        {
            try
            {
                var news = await _newsService.SearchByTextAsync(text);
                return Ok(news);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in SearchByText");
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search news with text {Text}.", text);
                return Problem(detail: ex.Message, statusCode: 500, title: "Unexpected error", instance: HttpContext?.Request?.Path.Value);
            }
        }


        /// <summary>
        /// Return latest news for distinct instruments.
        /// </summary>
        [HttpGet("public/latest")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Latest distinct-instrument news", Description = "Returns latest news for distinct instruments.")]
        public async Task<ActionResult<List<NewsArticle>>> GetLatestDistinctInstruments(
            [FromQuery][Range(1, 100)] int instruments = 5,
            CancellationToken ct = default)
        {
            try
            {
                var news = await _newsService.GetLatestDistinctInstrumentsAsync(instruments);
                return Ok(news);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetLatestDistinctInstruments");
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch latest public news.");
                return Problem(detail: ex.Message, statusCode: 500, title: "Unexpected error", instance: HttpContext?.Request?.Path.Value);
            }
        }
    }

}