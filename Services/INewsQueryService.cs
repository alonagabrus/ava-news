
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Services
{
    public interface INewsQueryService
    {
        Task<List<NewsArticle>> GetAllAsync();
        Task<List<NewsArticle>> GetFromLastNDaysAsync(int days);
        Task<List<NewsArticle>> GetByInstrumentAsync(string instrument, int limit);
        Task<List<NewsArticle>> SearchByTextAsync(string text);
        Task<List<NewsArticle>> GetLatestDistinctInstrumentsAsync(int instrumentCount);

    }
}