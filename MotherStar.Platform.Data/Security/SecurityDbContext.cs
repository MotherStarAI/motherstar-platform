using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotherStar.Platform.Domain.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Data.Security
{
    public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext<AppUser>(options)
    {
    }
}
