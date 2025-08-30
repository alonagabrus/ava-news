
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Services
{
    public interface IEnrichmentService
    {
        Task<List<NewsArticle>> EnrichAsync(PolygonResponseDto response, string provider = "Polygon");

    }
}