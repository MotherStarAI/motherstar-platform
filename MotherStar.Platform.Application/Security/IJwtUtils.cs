using MotherStar.Platform.Domain.Security.Models;

public interface IJwtUtils
{
    string GenerateToken(User user);
    int? ValidateToken(string token);
}