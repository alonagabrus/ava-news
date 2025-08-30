using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<bool> IsSubscribedAsync(string email);
        Task AddOrReactivateAsync(string email);
    }
}