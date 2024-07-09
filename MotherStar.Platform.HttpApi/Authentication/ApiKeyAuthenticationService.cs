using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace MotherStar.Platform.HttpApi.Authentication
{
    public class ApiKeyAuthenticationService : IApiKeyAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> IsValidAsync(string challengeApiKey)
        {
            string apiKey = _configuration.GetValue<string>(ApiKeyDefaults.ApiKeyName);

            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            if (challengeApiKey == null) throw new ArgumentNullException(nameof(challengeApiKey));

            if (apiKey == challengeApiKey)
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}
