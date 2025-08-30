using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Services
{
    public interface ISubscriptionService
    {
        Task<SubscriptionResult> SubscribeAsync(string email);
    }
}