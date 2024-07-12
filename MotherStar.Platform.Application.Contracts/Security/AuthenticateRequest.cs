using System.ComponentModel.DataAnnotations;

namespace MotherStar.Platform.Application.Contracts.Security
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}