using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.Security.Models
{
    public class AppUser : IdentityUser
    {
        public IEnumerable<IdentityRole>? Roles { get; set; }
    }
}
