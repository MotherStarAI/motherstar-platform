using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MotherStar.Platform.HttpApi.Authentication
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthOptions>
    {
        private readonly IApiKeyAuthenticationService _authenticationService;

        public ApiKeyAuthHandler(
            IOptionsMonitor<ApiKeyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IApiKeyAuthenticationService authenticationService)
            : base(options, logger, encoder)
        {
            _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(ApiKeyDefaults.AuthorizationHeaderName))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[ApiKeyDefaults.AuthorizationHeaderName], out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            if (!ApiKeyDefaults.ApiKeyAuthSchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                //No ApiKey authentication header
                return AuthenticateResult.NoResult();
            }
            if (headerValue.Parameter is null)
            {
                //Missing key
                return AuthenticateResult.Fail("Missing apiKey");
            }

            bool isValid = await _authenticationService.IsValidAsync(headerValue.Parameter);

            if (!isValid)
            {
                return AuthenticateResult.Fail("Invalid apiKey");
            }
            var claims = new[] { new Claim(ClaimTypes.Name, "Service") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"{ApiKeyDefaults.ApiKeyAuthSchemeName} \", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
    }
}
