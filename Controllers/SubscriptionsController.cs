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
            /* PseudoCode:
             * - Validate request (check Email not null/empty/invalid)
             * - Call _service.SubscribeAsync(request.Email)
             * - If success -> return 200 OK with { msg = "User was subscribed successfully" }
             * - If failure -> return appropriate error status (400/500) with message
             */
            throw new NotImplementedException("pseudocode provided");

        }

    }
}