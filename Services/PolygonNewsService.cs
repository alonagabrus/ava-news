
using AvaTradeNews.Api.Config;
using AvaTradeNews.Api.DTO;
using Microsoft.Extensions.Options;
using AvaTradeNews.Api.Repositories;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace AvaTradeNews.Api.Services
{
    public class PolygonNewsService : INewsExternalProviderService
    {
        private readonly HttpClient _http;
        private readonly PolygonProviderOptions _config;
        private readonly ILogger<PolygonNewsService> _logger;
        private readonly INewsRepository _repo;
        private readonly IEnrichmentService _enrichmentService;

        public PolygonNewsService(HttpClient http, IOptions<PolygonProviderOptions> config,
            ILogger<PolygonNewsService> logger, INewsRepository repo, IEnrichmentService enrichmentService)
        {
            _http = http;
            _config = config.Value;
            _logger = logger;
            _repo = repo;
            _enrichmentService = enrichmentService;
        }


        public async Task GetLatestNewsAsync(DateTime? lastPublishedDateTime = null, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Starting news fetch from Polygon");

                var articles = await FetchFilteredArticles(lastPublishedDateTime, ct);
                if (articles.Count == 0)
                {
                    _logger.LogInformation("No new articles found");
                    return;
                }

                await ProcessArticles(articles, ct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch latest news from Polygon");
            }
        }

        private async Task<List<NewsArticleDto>> FetchFilteredArticles(DateTime? lastPublishedDateTime, CancellationToken ct)
        {
            var cutoffDate = GetCutoffDate(lastPublishedDateTime);
            var response = await FetchNewsFromApi(ct);

            if (!IsValidResponse(response))
                return new List<NewsArticleDto>();

            return FilterArticlesByDate(response.Articles, cutoffDate);
        }

        private async Task ProcessArticles(List<NewsArticleDto> articles, CancellationToken ct)
        {
            _logger.LogInformation($"Processing {articles.Count} articles");
            try
            {
                await EnrichAndSaveArticles(articles, ct);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("EnrichAndSaveArticles was cancelled");
                throw;
            }
        }

        private DateTime GetCutoffDate(DateTime? lastPublishedDateTime)
        {
            return lastPublishedDateTime ?? DateTimeOffset.UtcNow.AddDays(-1).DateTime;
        }

        private async Task<PolygonResponseDto> FetchNewsFromApi(CancellationToken ct)
        {
            var url = $"{_config.BaseUrl?.TrimEnd('/')}/{_config.NewsEndpoint?.TrimStart('/')}?order={_config.Order ?? "desc"}&sort={_config.Sort ?? "published_utc"}&limit={(_config.Limit > 0 ? _config.Limit : 100)}&apiKey={Uri.EscapeDataString(_config.ApiKey ?? string.Empty)}";

            return await _http.GetFromJsonAsync<PolygonResponseDto>(url, ct) ?? new PolygonResponseDto();
        }

        private async Task EnrichAndSaveArticles(List<NewsArticleDto> articles, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            _logger.LogInformation("Got {ArticleCount} news articles", articles.Count);
            var responseDto = new PolygonResponseDto { Articles = articles };
            var enrichedArticles = await _enrichmentService.EnrichAsync(responseDto, "Polygon");
            ct.ThrowIfCancellationRequested();

            if (enrichedArticles.Count > 0)
            {
                await _repo.SaveNewAsync(enrichedArticles);
            }
        }


        private List<NewsArticleDto> FilterArticlesByDate(List<NewsArticleDto> articles, DateTime cutoffDate)
        {
            return articles.Where(a => a.PublishedUtc > cutoffDate).ToList();
        }


        private bool IsValidResponse(PolygonResponseDto response)
        {
            if (response.Status.ToLower() != "ok")
            {
                _logger.LogWarning("Get latest news from Polygon returned unsuccessful status ({Status})", response.Status);
                return false;
            }

            if (response.Articles == null || response.Articles.Count == 0)
            {
                _logger.LogInformation("No new articles to save");
                return false;
            }

            return true;
        }
    }


}