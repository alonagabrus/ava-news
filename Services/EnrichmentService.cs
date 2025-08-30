using Microsoft.Extensions.Caching.Memory;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Services
{
    public class EnrichmentService : IEnrichmentService
    {
        private readonly ILogger<EnrichmentService> _logger;
        private readonly IMemoryCache _cache;
        private static readonly Dictionary<string, PolygonTickerDto> _tickerCache = new();
        private static readonly string CachePrefix = "polygon:ticker:";
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromHours(1);

        public EnrichmentService(ILogger<EnrichmentService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            InitializeTickerCache();
        }

        public Task<List<NewsArticle>> EnrichAsync(PolygonResponseDto response, string provider = "Polygon")
        {
            if (response?.Articles == null || response.Articles.Count == 0)
                return Task.FromResult(new List<NewsArticle>());

            var enrichedArticles = new List<NewsArticle>();

            foreach (var article in response.Articles)
            {
                try
                {
                    var enrichedArticle = new NewsArticle
                    {
                        ArticleUrl = article.ArticleUrl,
                        Content = article.Description,
                        CreatedDate = DateTime.UtcNow,
                        Instruments = article.Tickers ?? new List<string>(),
                        PublisherName = article.Publisher?.Name,
                        ProviderName = provider,
                        PublicationDate = article.PublishedUtc,
                        Keywords = article.Keywords,
                        ProviderExternalId = article.Id,
                        Title = article.Title,
                        InstrumentsMeta = new List<EnrichedInstrument>()
                    };

                    var tickers = (article.Tickers ?? new List<string>())
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Select(t => t.Trim().ToUpperInvariant())
                        .Distinct()
                        .ToList();

                    if (tickers.Count > 0)
                    {
                        foreach (var ticker in tickers)
                        {
                            var info = GetTickerInfo(ticker);
                            if (info != null)
                            {
                                enrichedArticle.InstrumentsMeta.Add(new EnrichedInstrument
                                {
                                    Ticker = info.Ticker,
                                    Name = info.Name,
                                    Market = info.Market,
                                    MarketCap = info.MarketCap,
                                    HomepageUrl = info.HomepageUrl,
                                    Description = info.Description
                                });
                            }
                        }
                    }

                    enrichedArticles.Add(enrichedArticle);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Enriching article with Id {Id} ({Title}) failed.", article.Id, article.Title);
                }
            }

            return Task.FromResult(enrichedArticles);
        }

        private PolygonTickerDto? GetTickerInfo(string ticker)
        {
            var key = CachePrefix + ticker;

            if (_cache.TryGetValue(key, out PolygonTickerDto cached))
                return cached;

            if (_tickerCache.TryGetValue(ticker.ToUpperInvariant(), out var tickerData))
            {
                _cache.Set(key, tickerData, CacheExpirationTime);
                return tickerData;
            }

            return null;
        }

        /// <summary>
        /// Defined mock data for enrich the tickers
        /// </summary>
        private static void InitializeTickerCache()
        {
            _tickerCache["AAPL"] = new PolygonTickerDto
            {
                Ticker = "AAPL",
                Name = "Apple Inc.",
                Market = "stocks",
                MarketCap = 3000000000000m,
                HomepageUrl = "https://www.apple.com",
                Description = "Apple Inc. designs, manufactures, and markets smartphones, personal computers, tablets, wearables, and accessories worldwide."
            };

            _tickerCache["GOOGL"] = new PolygonTickerDto
            {
                Ticker = "GOOGL",
                Name = "Alphabet Inc.",
                Market = "stocks",
                MarketCap = 2100000000000m,
                HomepageUrl = "https://www.google.com",
                Description = "Alphabet Inc. provides various products and platforms worldwide."
            };

            _tickerCache["MSFT"] = new PolygonTickerDto
            {
                Ticker = "MSFT",
                Name = "Microsoft Corporation",
                Market = "stocks",
                MarketCap = 2800000000000m,
                HomepageUrl = "https://www.microsoft.com",
                Description = "Microsoft Corporation develops, licenses, and supports software, services, devices, and solutions worldwide."
            };

            _tickerCache["TSLA"] = new PolygonTickerDto
            {
                Ticker = "TSLA",
                Name = "Tesla, Inc.",
                Market = "stocks",
                MarketCap = 800000000000m,
                HomepageUrl = "https://www.tesla.com",
                Description = "Tesla, Inc. designs, develops, manufactures, leases, and sells electric vehicles."
            };

            _tickerCache["NVDA"] = new PolygonTickerDto
            {
                Ticker = "NVDA",
                Name = "NVIDIA Corporation",
                Market = "stocks",
                MarketCap = 1700000000000m,
                HomepageUrl = "https://www.nvidia.com",
                Description = "NVIDIA Corporation operates as a computing company worldwide."
            };
        }
    }
}