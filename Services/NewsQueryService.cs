
using AvaTradeNews.Api.Models;
using AvaTradeNews.Api.Repositories;

namespace AvaTradeNews.Api.Services
{
    public class NewsQueryService : INewsQueryService
    {
        private readonly INewsRepository _repository;

        public NewsQueryService(INewsRepository repo)
        {
            _repository = repo;
        }
        public Task<List<NewsArticle>> GetAllAsync()
        {
            return _repository.GetAll();
        }

        public Task<List<NewsArticle>> GetByInstrumentAsync(string instrument, int limit)
        {
            return _repository.GetByInstrumentAsync(instrument, limit);
        }

        public Task<List<NewsArticle>> GetFromLastNDaysAsync(int days)
        {
            var fromDate = DateTime.UtcNow.AddDays(-days);
            var toDate = DateTime.UtcNow;
            return _repository.GetByDateRangeAsync(fromDate, toDate);
        }
        /// <summary>
        /// Retrieves the latest news articles, ensuring that only the most recent article
        /// is selected per instrument, and then returns a limited number of distinct instruments.
        /// </summary>
        /// <param name="instrumentCount">The maximum number of distinct instruments to return.</param>
        /// <returns>A list of the latest NewsArticle objects, one per instrument.</returns>
        public async Task<List<NewsArticle>> GetLatestDistinctInstrumentsAsync(int instrumentCount)
        {
            var allNews = await _repository.GetAll();
            var articlesByInstrument = allNews
                .SelectMany(a => (a.Instruments ?? new List<string>()).Select(i => new { Article = a, Instrument = i }))
                .GroupBy(x => x.Instrument, StringComparer.OrdinalIgnoreCase);

            var latestArticlePerInstrument = articlesByInstrument
                .Select(g => g.OrderByDescending(x => x.Article.PublicationDate).First().Article);

            return latestArticlePerInstrument
                .OrderByDescending(a => a.PublicationDate)
                .Take(instrumentCount)
                .ToList();
        }



        public Task<List<NewsArticle>> SearchByTextAsync(string text)
        {
            return _repository.GetByTextAsync(text);
        }
    }
}