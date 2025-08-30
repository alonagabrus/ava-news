
using AvaTradeNews.Api.Models;
using AvaTradeNews.Api.Repositories;


namespace AvaTradeNews.Api.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUserRepository _users;
        private readonly ISubscriptionRepository _repo;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(IUserRepository users, ISubscriptionRepository subs, ILogger<SubscriptionService> logger)
        {
            _users = users;
            _repo = subs;
            _logger = logger;
        }
        public Task<SubscriptionResult> SubscribeAsync(string email)
        {
            //**PseudoCode**
            // validate email
            //if not valid-> return BadRequest;
            //Else-  call _repo.Subscribe
            //return the result
            throw new NotImplementedException("PseudoCode explained");
        }
    }
}