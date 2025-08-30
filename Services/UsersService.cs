using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;
using AvaTradeNews.Api.Repositories;

namespace AvaTradeNews.Api.Services
{
    public class UserService : IUsersService
    {
        private readonly IUserRepository _userRepository;

        // Constructor receives repository via DI
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            /* PseudoCode:
             * - Validate userDto (check required fields)
             * - Map UserDto to User entity (keep password as-is for now)
             * - Save User entity via _userRepository.AddAsync(user)
             * - Return the created User
             */
            throw new NotImplementedException("Pseudocode only");
        }

        public async Task<User?> LoginAsync(UserDto userDto)
        {
            /* PseudoCode:
             * - Validate userDto (email/password not empty)
             * - Retrieve user from _userRepository.GetByEmailAsync(userDto.Email)
             * - If user not found -> return null
             * - Compare provided password with user.Password (plain text check)
             * - If mismatch -> return null
             * - If match -> return User
             */
            throw new NotImplementedException("Pseudocode only");
        }

        public async Task<User?> GetByUsernameAsync(string email)
        {
            /* PseudoCode:
             * - Validate email (not null/empty)
             * - Call _userRepository.GetByEmailAsync(email)
             * - Return user (or null if not found)
             */
            throw new NotImplementedException("Pseudocode only");
        }
    }

}