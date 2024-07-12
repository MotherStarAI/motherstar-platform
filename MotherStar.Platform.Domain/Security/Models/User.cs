using RCommon.Entities;
using System.Text.Json.Serialization;

namespace MotherStar.Platform.Domain.Security.Models
{
    public class User : BusinessEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
