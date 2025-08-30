using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, User> _users = new();

        public void AddUser(User user)
        {
            if (!_users.TryAdd(user.Email, user))
                throw new InvalidOperationException($"User with email '{user.Email}' already exists.");
        }

        public User? GetByUsername(string username)
            => _users.TryGetValue(username, out var user) ? user : null;
    }
}