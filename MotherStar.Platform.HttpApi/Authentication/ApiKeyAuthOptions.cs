using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;

namespace MotherStar.Platform.HttpApi.Authentication
{
    public class ApiKeyAuthOptions : AuthenticationSchemeOptions
    {
        public string Scheme => ApiKeyDefaults.ApiKeyAuthSchemeName;
        public StringValues AuthKey { get; set; }
    }
}
