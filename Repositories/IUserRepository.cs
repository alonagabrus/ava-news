using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User? GetByUsername(string username);

    }
}