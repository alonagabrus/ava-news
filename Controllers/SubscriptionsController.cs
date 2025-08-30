using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaTradeNews.Api.DTO;
using AvaTradeNews.Api.Repositories;
using AvaTradeNews.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AvaTradeNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _service;

        public SubscriptionsController(ISubscriptionService service) { _service = service; }

        [HttpPost]
        [SwaggerOperation(Summary = "Subscribe user", Description = "Subscribe user to newsletter")]
        public async Task<IActionResult> Subscribe([FromBody] UserSubscriptionDto request)
        {
            /* **PseudoCode
           call _service SubscribeAsync(request.email)
           return Results status code

            updated existing - OK
           return StatusCode(200, new { msg = "User was subscribed successfully" }); */
            throw new NotImplementedException("pseudocode provided");

        }

    }
}