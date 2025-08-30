

namespace AvaTradeNews.Api.Services
{
    public interface INewsExternalProviderService
    {
        Task<DateTime?> GetLatestNewsAsync(DateTime? lastPublishedDateTime = null, CancellationToken ct = default);

    }
}