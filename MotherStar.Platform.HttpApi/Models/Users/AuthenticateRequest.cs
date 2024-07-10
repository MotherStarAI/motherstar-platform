using System.ComponentModel.DataAnnotations;

namespace MotherStar.Platform.HttpApi.Models.Users
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}