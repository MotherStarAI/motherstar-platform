using Microsoft.Extensions.DependencyInjection;
using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Security.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddSecurityApplicationServices(this IServiceCollection services)
        {
            // Add Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IJwtUtils, JwtUtils>();

            // AutoMapper Mapping Profiles
            services.AddAutoMapper(x => // Where all of our DTO mapping occurs
            {
                x.AddProfile<SecurityAutoMapperProfile>();
            });
        }
    }
}
