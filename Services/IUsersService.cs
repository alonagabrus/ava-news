using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.Services
{
    public interface IUsersService
    {
        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="userDto">The user data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the created User entity.</returns>
        Task<User> AddUserAsync(UserDto userDto);

        /// <summary>
        /// Logs in a user asynchronously by validating credentials.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing login credentials.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the authenticated User entity, or null if invalid.</returns>
        Task<User?> LoginAsync(UserDto userDto);

        /// <summary>
        /// Retrieves a user asynchronously by their email (username).
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the User entity if found, otherwise null.</returns>
        Task<User?> GetByUsernameAsync(string email);
    }

}