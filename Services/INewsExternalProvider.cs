

namespace AvaTradeNews.Api.Services
{
    public interface INewsExternalProviderService
    {
        Task GetLatestNewsAsync(DateTime? lastPublishedDateTime = null, CancellationToken ct = default);

    }
}