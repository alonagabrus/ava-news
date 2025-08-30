using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Models
{
    public class UserSubscription
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = "";
        public string? Instrument { get; set; }
        public DateTime SubscribedUtc { get; set; } = DateTime.UtcNow;
    }
}