using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Repositories
{
    public interface INewsRepository
    {
        Task<int> SaveNewAsync(List<NewsArticle> articles, CancellationToken ct = default);
        Task<NewsArticle?> FindByIdAsync(string id);
        Task<List<NewsArticle>> GetAll();
        Task<List<NewsArticle>> GetByTextAsync(string text);
        Task<List<NewsArticle>> GetByInstrumentAsync(string code, int limit);
        Task<List<NewsArticle>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
    }
}