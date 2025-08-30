using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AvaTradeNews.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login application user")]
        public IActionResult Login(User request)
        {
            try
            {
                // not implemented
                throw new NotImplementedException("Not implemented");
            }
            catch (Exception ex)
            {

                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}