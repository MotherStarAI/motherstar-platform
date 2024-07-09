using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace MotherStar.Platform.HttpApi.Authentication.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeyAuth<TAuthService>(this AuthenticationBuilder builder)
        where TAuthService : class, IApiKeyAuthenticationService
        {
            return builder.AddApiKeyAuth<TAuthService>(ApiKeyDefaults.ApiKeyAuthSchemeName, _ => { });
        }

        public static AuthenticationBuilder AddApiKeyAuth<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
            where TAuthService : class, IApiKeyAuthenticationService
        {
            return builder.AddApiKeyAuth<TAuthService>(authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddApiKeyAuth<TAuthService>(this AuthenticationBuilder builder, Action<ApiKeyAuthOptions> configureOptions)
            where TAuthService : class, IApiKeyAuthenticationService
        {
            return builder.AddApiKeyAuth<TAuthService>(ApiKeyDefaults.ApiKeyAuthSchemeName, configureOptions);
        }

        public static AuthenticationBuilder AddApiKeyAuth<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyAuthOptions> configureOptions)
            where TAuthService : class, IApiKeyAuthenticationService
        {
            builder.Services.AddSingleton<IPostConfigureOptions<ApiKeyAuthOptions>, ApiKeyAuthenticationPostConfigureOptions>();
            builder.Services.AddTransient<IApiKeyAuthenticationService, TAuthService>();

            return builder.AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(
                authenticationScheme, configureOptions);
        }
    }
}
