using System.Threading.Tasks;

namespace MotherStar.Platform.HttpApi.Authentication
{
    public interface IApiKeyAuthenticationService
    {
        Task<bool> IsValidAsync(string apiKey);
    }
}
