using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AvaTradeNews.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]

        public IActionResult Login(UserDto request)
        {
            try
            {
                /* PseudoCode:
             * - Validate request (username & password not empty)
             * - Lookup user by username
             * - If not found -> return Unauthorized
             * - Verify password against stored hash
             * - If invalid -> return Unauthorized
             * - Generate JWT token
             * - Return 200 OK with token + user details
             */
                throw new NotImplementedException("Not implemented");
            }
            catch (Exception ex)
            {

                return Unauthorized(new { error = ex.Message });
            }
        }
    }

}