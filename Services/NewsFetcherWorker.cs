using AvaTradeNews.Api.Config;
using Microsoft.Extensions.Options;

namespace AvaTradeNews.Api.Services
{
    public class NewsFetcherWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NewsFetcherWorker> _logger;
        private readonly TimeSpan _interval;
        private DateTime? _lastPublishedDateTime;
        private const int DefaultLookbackDays = 3;


        public NewsFetcherWorker(ILogger<NewsFetcherWorker> logger, IServiceProvider sp, IOptions<NewsFetcherOptions> config)
        {
            _logger = logger;
            _serviceProvider = sp;

            var minutes = config.Value.IntervalInMinutes <= 0 ? 60 : config.Value.IntervalInMinutes;
            _interval = TimeSpan.FromMinutes(minutes);

        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogDebug("News fetching worker execution started");

            ct.Register(() => _logger.LogInformation("Cancellation token triggered.News fetching worker is stopping."));
            var timer = new PeriodicTimer(_interval);
            do
            {
                try
                {
                    await FetchNewsAsync(ct);

                    /* PseudoCode:
                    * - Retrieve all subscribed user emails from the repository
                    * - Use the subscription channel's SendNewsletter method
                    *   - Pass in the list of subscribed emails
                    *   - Include the newsletter content to be sent
                    */
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "News fetching worker execution failed");

                }
            }
            while (!ct.IsCancellationRequested && await timer.WaitForNextTickAsync(ct));
        }
        /// <summary>
        /// Fetches the latest news from the external provider and updates the watermark (_lastPublishedDateTime).
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation if needed.</param>
        private async Task FetchNewsAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching news process started");
                if (!_lastPublishedDateTime.HasValue || _lastPublishedDateTime == default)
                {
                    _lastPublishedDateTime = DateTimeOffset.UtcNow.AddDays(-DefaultLookbackDays).DateTime;
                }
                // Create a new DI scope so scoped services can be resolved safely
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Resolve the news provider service from the scope
                    var fetchService = scope.ServiceProvider.GetRequiredService<INewsExternalProviderService>();

                    // Fetch news newer than the current watermark
                    var newMax = await fetchService.GetLatestNewsAsync(_lastPublishedDateTime, ct);
                    // If a newer max publish date is found, update the watermark
                    if (newMax.HasValue && (!_lastPublishedDateTime.HasValue || newMax > _lastPublishedDateTime))
                    {
                        _lastPublishedDateTime = newMax;
                        _logger.LogInformation("Watermark advanced to {Watermark:O}", _lastPublishedDateTime);
                    }
                }

                _logger.LogInformation("Fetching news process finished successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fetching news process failed.");
            }
        }
    }
}