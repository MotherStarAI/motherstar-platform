using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MotherStar.Platform.Data.Security;
using MotherStar.Platform.Domain.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MotherStar.Platform.Data;
using MotherStar.Platform.Domain;

namespace MotherStar.Platform.Bootstrapper.Security.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure authorization
            services.AddAuthorizationBuilder();

            services.AddDbContext<SecurityDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(DataStoreNamesConst.SecurityDb));
                //options.UseSqlServer(configuration.GetConnectionString(DataStoreNamesConst.SecurityDb));
            });
            // Add identity and opt-in to endpoints
            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddApiEndpoints();
        }
    }
}
