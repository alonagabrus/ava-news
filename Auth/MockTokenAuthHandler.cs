using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AvaTradeNews.Api.Auth
{
    public class MockTokenAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public MockTokenAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, IConfiguration configuration)
            : base(options, logger, encoder)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var expected = _configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(expected))
                return Task.FromResult(AuthenticateResult.Fail("Jwt:Key is missing"));

            var header = Request.Headers.Authorization.ToString();
            if (header == $"Bearer {expected}")
            {
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "avatrade-identity") }, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("invalid token"));
        }
    }
}