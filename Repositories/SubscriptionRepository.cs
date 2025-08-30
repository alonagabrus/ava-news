using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private static ConcurrentDictionary<string, bool> _subscriptions = new(StringComparer.OrdinalIgnoreCase);

        public Task<bool> IsSubscribedAsync(string email)
        {
            var existsAndActive = _subscriptions.TryGetValue(email, out var active) && active == true;
            return Task.FromResult(existsAndActive);
        }

        public Task AddOrReactivateAsync(string email)
        {
            _subscriptions[email] = true;
            return Task.CompletedTask;
        }
    }
}