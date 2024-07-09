using Microsoft.Extensions.Options;

namespace MotherStar.Platform.HttpApi.Authentication
{
    public class ApiKeyAuthenticationPostConfigureOptions : IPostConfigureOptions<ApiKeyAuthOptions>
    {
        public void PostConfigure(string name, ApiKeyAuthOptions options) { } //Nothing to do
    };
}
