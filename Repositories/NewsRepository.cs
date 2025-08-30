using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ConcurrentDictionary<string, NewsArticle> _news = new(StringComparer.OrdinalIgnoreCase);

        public Task<NewsArticle?> FindByIdAsync(string id)
            => Task.FromResult(_news.TryGetValue(id, out var v) ? v : null);

        public Task<List<NewsArticle>> GetAll()
            => Task.FromResult(_news.Values.OrderByDescending(a => a.PublicationDate).ToList());

        public Task<List<NewsArticle>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var articles = _news.Values
                                    .Where(a => a.PublicationDate >= fromDate && a.PublicationDate <= toDate).OrderByDescending(a => a.PublicationDate).ToList();
            return Task.FromResult(articles);
        }

        public Task<List<NewsArticle>> GetByInstrumentAsync(string code, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Task.FromResult(new List<NewsArticle>());

            var articles = _news.Values
                                    .Where(a => a.Instruments != null &&
                            a.Instruments.Any(i => string.Equals(i, code, StringComparison.OrdinalIgnoreCase))).OrderByDescending(a => a.PublicationDate).Take(limit).ToList();
            return Task.FromResult(articles);
        }

        public Task<List<NewsArticle>> GetByTextAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Task.FromResult(new List<NewsArticle>());
            var articles = _news.Values.Where(a => (!string.IsNullOrEmpty(a.Title) && a.Title.Contains(text, StringComparison.OrdinalIgnoreCase)) ||
                                                (!string.IsNullOrEmpty(a.Content) && a.Content.Contains(text, StringComparison.OrdinalIgnoreCase)))
                                            .OrderByDescending(aa => aa.PublicationDate).ToList();
            return Task.FromResult(articles);
        }

        public Task<int> SaveNewAsync(List<NewsArticle> articles, CancellationToken ct = default)
        {
            if (articles == null) return Task.FromResult(0);

            var saved = 0;
            foreach (var a in articles)
            {
                ct.ThrowIfCancellationRequested();
                if (a == null) continue;

                if (string.IsNullOrWhiteSpace(a.ProviderExternalId))
                {
                    continue;
                }
                if (_news.TryAdd(a.ProviderExternalId, a))
                    saved++;
            }

            return Task.FromResult(saved);
        }

    }
}