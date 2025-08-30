using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Models
{
    public class SubscriptionResult
    {
        public string Email { get; init; } = string.Empty;
        public DateTime? SubscribedAt { get; init; } = DateTime.UtcNow;
    }
}