using System.ComponentModel.DataAnnotations;

namespace MotherStar.Platform.Web.Models.Account
{
    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}