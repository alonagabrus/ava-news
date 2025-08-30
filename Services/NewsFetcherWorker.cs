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

                    //TODO: send newletter to subscribers
                    /* PseudoCOde:
                        get all emails from repository which were subscribed to newsletter
                        SUbscription channel interface should be injected and initialized in this class constructor.
                        Call SendNewsLetter method with all relevant emails
                    */
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "News fetching worker execution failed");

                }
            }
            while (!ct.IsCancellationRequested && await timer.WaitForNextTickAsync(ct));
        }

        private async Task FetchNewsAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching news process started");
                if (!_lastPublishedDateTime.HasValue || _lastPublishedDateTime == default)
                {
                    _lastPublishedDateTime = DateTimeOffset.UtcNow.AddDays(-DefaultLookbackDays).DateTime;
                }
                using (var scope = _serviceProvider.CreateScope())
                {
                    var fetchService = scope.ServiceProvider.GetRequiredService<INewsExternalProviderService>();

                    await fetchService.GetLatestNewsAsync(_lastPublishedDateTime);
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